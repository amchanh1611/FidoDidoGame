using AutoMapper;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Modules.FidoDidos.Response;
using FidoDidoGame.Modules.Rank.Request;
using FidoDidoGame.Modules.Ranks.Services;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Services;
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
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly IBackgroundJobClient hangfire;
        private readonly IUserService userService;
        private readonly IRedisService redis;
        private readonly IRankService rankService;

        public FidoDidoService(IRepository repository, IMapper mapper, IBackgroundJobClient hangfire, IRedisService redis, IUserService userService, IRankService rankService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.hangfire = hangfire;
            this.userService = userService;
            this.redis = redis;
            this.rankService = rankService;
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

            List<Fido> fidos = repository.Fido.FindAll().OrderBy(x => x.PercentRand).ToList();
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

            return new FidoResponse(userId, fido!.Name, JsonSerializer.Deserialize<List<UserStatus>>(user.Status)!);
        }

        public void UpdateFidoPercent(int fidoId, UpdateFidoPercentRequest request)
        {
            List<Fido> fidos = repository.Fido.FindAll().OrderBy(x => x.PercentRand).ToList();
            Fido fido = fidos.Where(x => x.Id == fidoId).FirstOrDefault()!;


            fido.Percent += request.Percent!.Value;

            fido.PercentRand = fidos.TakeWhile(x => x.PercentRand < fido.PercentRand).Sum(x => x.Percent) + fido.Percent;

            int index = fidos.IndexOf(fido);

            fidos = fidos.OrderBy(x => x.PercentRand).ToList();

            for (int i = index; i < fidos.Count; i++)
            {
                fidos[i].PercentRand = fidos.TakeWhile(x => x.PercentRand < fidos[i].PercentRand).Sum(x => x.Percent) + fidos[i].Percent;
            }

            repository.Fido.UpdateMulti(fidos);
            repository.Fido.Save();
        }

        public DidoResponse Dido(int userId)
        {
            using IDbContextTransaction transaction = repository.FidoDido.Transaction();
            try
            {
                User user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;

                List<UserStatus> userStatus = JsonSerializer.Deserialize<List<UserStatus>>(user.Status)!;

                //Gets list Dido of Fido
                List<FidoDido> fidoDidos = repository.FidoDido.FindByCondition(x => x.FidoId == user.FidoId).Include(x => x.Dido).OrderBy(x => x.PercentRand).ToList();

                //Gets Item
                Random rand = new();
                int point = rand.Next(0, 100);

                FidoDido fidoDido = fidoDidos.Where(x => point < x.PercentRand).FirstOrDefault()!;

                //Gets Item Date
                DateTime date = DateTime.Now;

                //Create a object outPoint used to checking if FidoDido.Point can be Parse
                int outPoint = 0;
                if (int.TryParse(fidoDido.Point, out outPoint))
                {
                    //If user gets the Normal Item 

                    //Checking if User is getting x2
                    if (userStatus.Where(x => x == UserStatus.X2).Any())
                        outPoint *= 2;

                    //Update rank of user
                    rankService.UpdateRank(new UpdateRank { UserName = user.Name, Point = outPoint, Date = date, UserId = userId });
                }
                else
                {
                    //If user gets the Special Item 

                    UserStatus status = UserStatus.Normal;
                    status = fidoDido.Point switch
                    {
                        "x2" => UserStatus.X2,
                        "auto" => UserStatus.Auto,
                        "heal" => UserStatus.Heal,
                        _ => UserStatus.Ban,
                    };

                    //Update user status
                    userService.AddUserStatus(userId, status);

                    //Set schedule to delete user status
                    DateTime dateResetStatus = DateTime.Now.AddMinutes(1);
                    hangfire.Schedule(() => userService.DeleteUserStatus(userId, status), dateResetStatus);
                }

                //Add History gets item Of User
                rankService.CreatePointDetail(new CreatePointDetailRequest
                {
                    UserId = userId,
                    UserName = user.Name,
                    Point = fidoDido.Point,
                    Date = date,
                    IsX2 = outPoint == 0 ? fidoDido.Point : outPoint.ToString()
                });

                //Update user Fido
                user.FidoId = null;
                repository.User.Update(user);
                repository.User.Save();

                transaction.Commit();

                return new DidoResponse(fidoDido.Dido!.Name, outPoint == 0 ? fidoDido.Point : outPoint.ToString());
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new BadRequestException("Dido fail");
            }
        }
    }
}
