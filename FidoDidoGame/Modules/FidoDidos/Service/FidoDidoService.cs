using AutoMapper;
using FidoDidoGame.Common.Extensions;
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

namespace FidoDidoGame.Modules.FidoDidos.Service;

public interface IFidoDidoService
{
    void CreateFido(CreateFidoRequest request);
    void CreateMultiDido(List<string> didoNames);
    void CreateFidoDido(CreateFidoDidoRequest request);
    void UpdateFidoPercent(int fidoId, UpdateFidoPercentRequest request);
    FidoResponse Fido(long userId);
    DidoResponse Dido(long userId);
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
        repository.Save();
    }

    public void CreateMultiDido(List<string> didoNames)
    {
        List<Dido> didos = didoNames.Select(x => new Dido { Name = x }).ToList();
        repository.Dido.CreateMulti(didos);
        repository.Save();
    }

    public void CreateFido(CreateFidoRequest request)
    {
        Fido dido = mapper.Map<CreateFidoRequest, Fido>(request);

        dido.PercentRand = repository.Fido.FindAll().Sum(x => x.Percent) + request.Percent!.Value;

        repository.Fido.Create(dido);
        repository.Save();
    }

    public FidoResponse Fido(long userId)
    {

        //Use Redis gets user status
        List<SpecialStatus> userStatus = userService.GetUserStatuses(userId);

        if (userStatus.Where(x => x is SpecialStatus.Ban).Any())
            throw new BadRequestException("User is banned");

        User user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;


        //Gets list dido 
        List<Fido> fidos = repository.Fido.FindAll().OrderBy(x => x.PercentRand).ToList();

        //Gets Fido
        // <Start>
        int point = Helper.Random(0, 100);

        Fido fido = fidos.Where(x => point < x.PercentRand).FirstOrDefault()!;
        // </End>

        user!.FidoId = fido.Id;
        repository.User.Update(user);
        repository.Save();

        return new FidoResponse(userId, fido!.Name, userStatus);
    }

    public void UpdateFidoPercent(int fidoId, UpdateFidoPercentRequest request)
    {
        // Gets all fido then sort PercentRand ascending
        List<Fido> fidos = repository.Fido.FindAll().OrderBy(x => x.PercentRand).ToList();

        // Gets Fido need update
        Fido fido = fidos.Where(x => x.Id == fidoId).FirstOrDefault()!;

        // Update percent
        fido.Percent = request.Percent!.Value;

        // Update PercentRand
        // < Sumary >
        /// Sum all PercentRand from the beggining to before fido need update
        // < /Sumary >
        fido.PercentRand = fidos.TakeWhile(x => x.PercentRand < fido.PercentRand).Sum(x => x.Percent) + fido.Percent;

        // Take index of fido updated
        int index = fidos.IndexOf(fido);

        // Reorder list fido
        fidos = fidos.OrderBy(x => x.PercentRand).ToList();

        // Recaculation all PercentRand from fido updated
        for (int i = index; i < fidos.Count; i++)
        {
            fidos[i].PercentRand = fidos.TakeWhile(x => x.PercentRand < fidos[i].PercentRand).Sum(x => x.Percent) + fidos[i].Percent;
        }

        repository.Fido.UpdateMulti(fidos);
        repository.Save();
    }

    public DidoResponse Dido(long userId)
    {
        using IDbContextTransaction transaction = repository.Transaction();
        try
        {
            //Gets user status
            List<SpecialStatus> userStatus = userService.GetUserStatuses(userId);

            // Check if user is ban
            if (userStatus.Where(x => x is SpecialStatus.Ban).Any())
                throw new BadHttpRequestException("User is banned");

            User user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;
            
            //Gets list Dido of Fido
            List<FidoDido> fidoDidos = repository.FidoDido.FindByCondition(x => x.FidoId == user.FidoId).Include(x => x.Dido).OrderBy(x => x.PercentRand).ToList();

            #region Gets Dido
            int number = Helper.Random(0, 100);

            FidoDido fidoDido = fidoDidos.Where(x => number < x.PercentRand).FirstOrDefault()!;
            #endregion Gets Dido

            //Date Gets Item 
            DateTime date = DateTime.Now;

            //Create a object point used to store point
            int point = fidoDido.Point;
            if (fidoDido.SpecialStatus is SpecialStatus.Point)
            {
                //If user gets the Normal Item 

                //Checking if User is getting x2
                if (userStatus.Where(x => x == SpecialStatus.X2).Any())
                    point *= 2;

                //Update rank of user
                rankService.UpdateRank(new UpdateRank(user.Name, userId, fidoDido.Point, date));
            }
            else
            {
                //If user gets the Special Item 
                ///Add user status
                redis.Set($"User:{user.Id}:Status:{fidoDido.SpecialStatus}", fidoDido.SpecialStatus, DateTime.Now.AddMinutes(1));
            }

            //Add History gets item Of User
            rankService.CreatePointDetail(new CreatePointDetailRequest(userId, user.Name, fidoDido.Point, date, point, fidoDido.SpecialStatus));

            //Update user Fido
            user.FidoId = null;
            repository.User.Update(user);
            repository.Save();

            transaction.Commit();

            return new DidoResponse(fidoDido.Dido!.Name, fidoDido.SpecialStatus, point);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new BadRequestException("Dido fail");
        }
    }
}


