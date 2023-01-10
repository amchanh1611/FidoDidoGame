using AutoMapper;
using FidoDidoGame.Common.Jwt;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Rank.Entities;
using FidoDidoGame.Modules.Rank.Request;
using FidoDidoGame.Modules.Ranks.Services;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Modules.Users.Response;
using FidoDidoGame.Persistents.Redis.Entities;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
    void GetReward(int eventId);
    void CreateRole(CreateRoleRequest request);
    public JwtTokenResponse Login(LoginRequest request);
}
public class UserService : IUserService
{
    private readonly IRedisService redis;
    private readonly IRepository repository;
    private readonly IMapper mapper;
    private readonly IJwtServices jwt;
    private readonly IRankService rankService;

    public UserService(IRepository repository, IMapper mapper, IRedisService redis, IJwtServices jwt, IRankService rankService)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.redis = redis;
        this.jwt = jwt;
        this.rankService = rankService;
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

            DateTime date = DateTime.Now;

            Event round = repository.Event.FindByCondition(x => x.DateStart <= date && x.DateEnd >= date).FirstOrDefault()!;

            rankService.CreatePointOfRound(new CreatePointOfRoundRequest(user.Id, user.Name, 0, date, round.Id));

            transaction.Commit();
        }
        catch (Exception)
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
        User user = repository.User.FindByCondition(x => x.Id == userId).Include(x=>x.Role).FirstOrDefault()!;

        if (user.RefreshToken is null)
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
    public void GetReward(int eventId)
    {
        Event round = repository.Event.FindByCondition(x => x.Id == eventId).FirstOrDefault()!;

        List<UserRankOfRoundIn> rank = redis.ZSGet<UserRankOfRoundIn>($"{RankService.KeysRankOfDay}:Round:{eventId}");

        for (int i = 0; i < 3000; i++)
        {
            // Nếu danh sách người chơi ít hơn 3000 thì cho chạy tới cuối danh sách là dừng
            if (i >= rank.Count)
                break;

            // Nếu người chơi có điểm bằng 0 (không chơi) thì không phát quà
            if (rank[i].Point == 0)
                continue;

            Award award = Award.Consolation;

            switch (i)
            {
                case 0: award = Award.First; break;
                case 1: award = Award.Second; break;
                case 2: award = Award.Third; break;
                default: break;
            }

            CreateReward(new CreateRewardRequest(rank[i].UserId, award, round.DateStart, round.DateEnd));
        }
    }
    private void CreateReward(CreateRewardRequest request)
    {
        repository.Reward.Create(mapper.Map<CreateRewardRequest, Reward>(request));
        repository.Save();
    }
    public void CreateRole(CreateRoleRequest request)
    {
        repository.Role.Create(mapper.Map<CreateRoleRequest, Role>(request));
        repository.Save();
    }

    public JwtTokenResponse Login(LoginRequest request)
    {
        User user = repository.User.FindByCondition(x=>x.Name == request.UserName && x.Password == request.Password).Include(x=>x.Role).FirstOrDefault()!;

        return JwtGenerateToken(user.Id);
    }
}
