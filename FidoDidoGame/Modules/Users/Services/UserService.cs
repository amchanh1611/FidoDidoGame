using AutoMapper;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace FidoDidoGame.Modules.Users.Services;

public interface IUserService
{
    void Create(CreateUserRequest request);
    void Update(int userId, UpdateUserRequest request);
    List<SpecialStatus> GetUserStatuses(int userId);
    User Profile(int userId);
}
public class UserService : IUserService
{
    private readonly IRedisService redis;
    private readonly IRepository repository;
    private readonly IMapper mapper;
    public UserService(IRepository repository, IMapper mapper, IRedisService redis)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.redis = redis;
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
        catch(Exception ex)
        {
            transaction.Rollback();
            throw new BadRequestException("Create Fail");
        }
    }

    public List<SpecialStatus> GetUserStatuses(int userId)
    {
        return redis.GetAll<SpecialStatus>($"User:{userId}:Status:").Distinct().ToList();
    }

    public User Profile(int userId)
    {
        return repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;
    }

    public void Update(int userId, UpdateUserRequest request)
    {
        User? user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault();
        repository.User.Update(mapper.Map(request, user!));
        repository.Save();
    }
}
