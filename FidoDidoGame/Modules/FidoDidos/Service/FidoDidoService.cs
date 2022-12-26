using AutoMapper;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Modules.FidoDidos.Response;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Services;
using FidoDidoGame.Persistents.Redis.Entities;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace FidoDidoGame.Modules.FidoDidos.Service
{
    public interface IFidoDidoService
    {
        void CreateFido(CreateFidoRequest request);
        void CreateMultiDido(List<string> didoNames);
        void CreateFidoDido(CreateFidoDidoRequest request);
        void UpdateFidoPercent(int fidoId, UpdateFidoPercentRequest request);
        FidoResponse Fido(int userId);
        DidoResponse Dido(int userId);
    }
    public class FidoDidoService : IFidoDidoService
    {
        public const string KeysRankOfDay = "Rank";
        public const string KeyRankDetail = "History";
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly IBackgroundJobClient hangfire;
        private readonly IUserService userService;
        private readonly IRedisService redis;

        public FidoDidoService(IRepository repository, IMapper mapper, IBackgroundJobClient hangfire, IRedisService redis, IUserService userService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.hangfire = hangfire;
            this.userService = userService;
            this.redis = redis;
        }

        public void CreateFidoDido(CreateFidoDidoRequest request)
        {
            FidoDido fidoDido = mapper.Map<CreateFidoDidoRequest, FidoDido>(request);

            fidoDido.PercentRand = repository.FidoDido
                .FindByCondition(x => x.FidoId == request.FidoId).Sum(x => x.Percent) + request.Percent!.Value;
            
            repository.FidoDido.Create(fidoDido);
            repository.Fido.Save();
        }

        public void CreateMultiDido(List<string> didoNames)
        {
            List<Dido> didos = didoNames.Select(x => new Dido { Name = x }).ToList();
            repository.Dido.CreateMulti(didos);
            repository.Fido.Save();
        }

        public void CreateFido(CreateFidoRequest request)
        {
            Fido dido = mapper.Map<CreateFidoRequest, Fido>(request);

            dido.PercentRand = repository.Fido.FindAll().Sum(x => x.Percent) + request.Percent!.Value;

            repository.Fido.Create(dido);
            repository.Fido.Save();
        }

        public FidoResponse Fido(int userId)
        {
            User user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;

            List<Fido> fidos  = repository.Fido.FindAll().OrderBy(x=>x.PercentRand).ToList();
            Random rand = new();
            int point = rand.Next(0, 100);

            Fido fido = new();

            foreach (Fido item in fidos)
            {
                if (point <= item.PercentRand)
                {
                    fido = item;
                    break;
                }
            }

            user!.FidoId = fido.Id;
            repository.User.Update(user);
            repository.User.Save();

            return new FidoResponse(userId,fido!.Name, JsonSerializer.Deserialize<List<UserStatus>>(user.Status)!);
        }

        public void UpdateFidoPercent(int fidoId, UpdateFidoPercentRequest request)
        {
            var a = repository.Fido.FindAll().TakeWhile(x => x.Id == fidoId).AsEnumerable();

            Fido fido = repository.Fido.FindByCondition(x => x.Id == fidoId).FirstOrDefault()!;

            Fido fidoUpdate = mapper.Map(request, fido);
        }

        public DidoResponse Dido(int userId)
        {
            using IDbContextTransaction transaction = repository.FidoDido.Transaction();
            try
            {
                User user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;

                List<UserStatus> userStatus = JsonSerializer.Deserialize<List<UserStatus>>(user.Status)!;

                List<FidoDido> fidoDidos = repository.FidoDido.FindByCondition(x => x.FidoId == user.FidoId).Include(x=>x.Dido).OrderBy(x => x.PercentRand).ToList();

                Random rand = new();
                int point = rand.Next(0, 100);

                DateTime date = DateTime.UtcNow;

                FidoDido fidoDido = new();

                foreach (FidoDido item in fidoDidos)
                {
                    if (point < item.PercentRand)
                    {
                        fidoDido = item;
                        break;
                    }
                }

                //Create a object outPoint used to checking if FidoDido.Point can be Parse
                int outPoint = 0;
                if (int.TryParse(fidoDido.Point, out outPoint))
                {
                    //Checking if User is getting x2
                    if (userStatus.Where(x => x == UserStatus.X2).Any())
                        outPoint *= 2;

                    PointDetail pointDetail = repository.PointDetail.Create(new PointDetail { UserId = user.Id, Date = date, Point = outPoint.ToString() });

                    PointOfDay pointOfDay = repository.PointOfDay.FindByCondition(x => x.UserId == user.Id && x.Date.Date == date.Date).FirstOrDefault()!;

                    UserRankOfDayIn userRankOfDayIn = new();

                    if (pointOfDay is null)
                    {
                        pointOfDay = repository.PointOfDay.Create(new PointOfDay { UserId = user.Id, Date = date, Point = outPoint });
                    }
                    else
                    {
                        //Remove old member in sortset in Redis if PointOfDay already exist
                        userRankOfDayIn = new(pointOfDay.Date, userId, user.Name);
                        redis.ZSDelete(KeysRankOfDay, userRankOfDayIn);

                        //Update point in MySql 
                        pointOfDay.Point += outPoint;
                        pointOfDay.Date = date;
                        repository.PointOfDay.Update(pointOfDay);
                    }

                    repository.User.Save();

                    //create a PointOfDay to save to Redis
                    userRankOfDayIn = new(pointOfDay.Date, userId, user.Name);
                    UserRankDetailIn userRankDetailIn = new(date, userId, user.Name, fidoDido.Point);
                    redis.ZSIncre(KeysRankOfDay, pointOfDay.Point, userRankOfDayIn);
                    redis.Set($"{KeyRankDetail}:{userId}:{pointDetail.Id}", userRankDetailIn);

                }
                else
                {
                    UserStatus status = UserStatus.Normal;
                    switch (fidoDido.Point)
                    {
                        case "x2":
                            status = UserStatus.X2;
                            break;
                        case "auto":
                            status = UserStatus.Auto;
                            break;
                        case "heal":
                            status = UserStatus.Heal;
                            break;
                        default:
                            status = UserStatus.Ban;
                            break;

                    }
                    userService.AddUserStatus(userId, status);

                    DateTime dateResetStatus = DateTime.Now.AddMinutes(1);
                    hangfire.Schedule(() => userService.DeleteUserStatus(userId, status), dateResetStatus);

                    PointDetail pointDetail = repository.PointDetail.Create(new PointDetail { UserId = user.Id, Date = date, Point = fidoDido.Point.ToString() });
                    repository.PointDetail.Save();

                    UserRankDetailIn userRankDetailIn = new(date, userId, user.Name, fidoDido.Point);
                    redis.Set($"{KeyRankDetail}{userId}:{pointDetail.Id}", userRankDetailIn);

                }

                user.FidoId = null;
                repository.User.Update(user);
                repository.User.Save();
                transaction.Commit();
                return new DidoResponse(fidoDido.Dido!.Name, fidoDido.Point);
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw new BadRequestException("Dido fail");
            }
        }
    }
}
