using AutoMapper;
using FidoDidoGame.Common.Jwt;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.FidoDidos.Service;
using FidoDidoGame.Modules.Ranks.Services;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Modules.Users.Response;
using FidoDidoGame.Persistents.Redis.Entities;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace FidoDidoGame.Modules.Users.Services;

public interface IUserService
{
    void Create(CreateUserRequest request);
    void Update(long userId, UpdateUserRequest request);
    List<SpecialStatus> GetUserStatuses(long userId);
    JwtTokenResponse JwtGenerateToken(long userId);
    string RefreshToken(string refreshToken);
    User Profile(long userId);
    bool FindById(long userId);
    void GetReward();
}
public class UserService : IUserService
{
    private readonly IRedisService redis;
    private readonly IRepository repository;
    private readonly IMapper mapper;
    private readonly IJwtServices jwt;
    
    public UserService(IRepository repository, IMapper mapper, IRedisService redis, IJwtServices jwt)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.redis = redis;
        this.jwt = jwt;
    }

    public void Create(CreateUserRequest request)
    {
        using IDbContextTransaction transaction = repository.Transaction();
        try
        {
            User user = repository.User.Create(mapper.Map<CreateUserRequest, User>(request));
            repository.Save();

            // Set default UserStatus is normal 
            redis.Set($"User:{user.Id}:Status:{SpecialStatus.Normal}", SpecialStatus.Normal);

            transaction.Commit();
        }
        catch(Exception)
        {
            transaction.Rollback();
            throw new BadRequestException("Create Fail");
        }
    }

    public bool FindById(long userId)
    {
        return repository.User.FindByCondition(x => x.Id == userId).Any();
    }

    public List<SpecialStatus> GetUserStatuses(long userId)
    {
        return redis.GetAll<SpecialStatus>($"User:{userId}:Status:").Distinct().ToList();
    }

    public JwtTokenResponse JwtGenerateToken(long userId)
    {
        User user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;

        if(user.RefreshToken is null)
        {
            user.RefreshToken = jwt.GenerateRefreshToken();
            user = repository.User.Update(user);
            repository.Save();
        }

        return new JwtTokenResponse { AccessToken = jwt.GenerateAccessToken(user), RefreshToken = user.RefreshToken };
    }

    public User Profile(long userId)
    {
        return repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;
    }

    public string RefreshToken(string refreshToken)
    {
        User user = repository.User.FindByCondition(x => x.RefreshToken == refreshToken).FirstOrDefault()!;

        if (user.RefreshToken is null)
            throw new BadRequestException("Unauthorize");

        if (!jwt.ValidateJwtToken(user.RefreshToken))
            throw new BadRequestException("Token has expired");

        return jwt.GenerateAccessToken(user);
    }

    public void Update(long userId, UpdateUserRequest request)
    {
        User? user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault();
        repository.User.Update(mapper.Map(request, user!));
        repository.Save();
    }
    public void GetReward()
    {
        DateTime dateEnd = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        DateTime dateStart = DateTime.Now.Date.AddDays(-7);

        List<UserRankOfDayIn> rank = redis.ZSGet<UserRankOfDayIn>(RankService.KeysRankOfDay)
            .Where(x => x.Date >= dateStart && x.Date <= dateEnd)
            .GroupBy(key => key.UserId)
            .Select(grp => new UserRankOfDayIn
            (
                grp.Select(x => x.DateMiliSecond).LastOrDefault(), 
                grp.Select(x => x.UserName).FirstOrDefault()!, 
                grp.Sum(x => x.Point), grp.Key,
                grp.Select(x => x.Date).LastOrDefault())
            )
            .OrderByDescending(x => x.Point).ThenBy(x=>x.Date)
            .ToList();

        repository.Reward.Create(new Reward(rank[0].UserId, Award.First, dateStart, dateEnd));

        repository.Reward.Create(new Reward(rank[1].UserId, Award.Second, dateStart, dateEnd));

        repository.Reward.Create(new Reward(rank[2].UserId, Award.Third, dateStart, dateEnd));

        for(int i = 3; i < 3000; i++)
        {
            // Nếu danh sách người chơi ít hơn 3000 thì cho chạy tới cuối danh sách là dừng
            if(i == rank.Count - 1)
                break;

            // Nếu người chơi có điểm bằng 0 (không chơi) thì không phát quà
            if (rank[i].Point == 0)
                continue;

            repository.Reward.Create(new Reward(rank[i].UserId, Award.Consolation, dateStart, dateEnd));
        }
    }
}
