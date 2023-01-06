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
        this.fidoDidoService = fidoDidoService;
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
        List<UserRankOfDayIn> rank = redis.ZSGet<UserRankOfDayIn>(RankService.KeysRankOfDay);

        repository.Reward.Create(new Reward(rank[0].UserId, Award.First,))
    }
}
