using AutoMapper;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Persistents.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace FidoDidoGame.Modules.Users.Services
{
    public interface IUserService
    {
        void Create(CreateUserRequest request);
        void Update(int userId, UpdateUserRequest request);
        void AddUserStatus(int userId, UserStatus status);
        void DeleteUserStatus(int userId, UserStatus status);
        User Profile(int userId);
    }
    public class UserService : IUserService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        public UserService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public void Create(CreateUserRequest request)
        {
            using IDbContextTransaction transaction = repository.User.Transaction();
            try
            {
                User user = repository.User.Create(mapper.Map<CreateUserRequest, User>(request));
                List<UserStatus> userStatus = new() { UserStatus.Normal};
                user.Status = JsonSerializer.Serialize(userStatus);
                repository.User.Save();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw new BadRequestException("Create Fail");
            }
        }

        public void DeleteUserStatus(int userId, UserStatus status)
        {
            User user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;
            List<UserStatus> userStatus = JsonSerializer.Deserialize<List<UserStatus>>(user.Status)!;
            if (status == UserStatus.Ban)
            {
                userStatus.Remove(UserStatus.Ban);
                userStatus.Add(UserStatus.Normal);
                user.Status = JsonSerializer.Serialize(userStatus);
            }
            else
            {
                userStatus.Remove(status);
                user.Status = JsonSerializer.Serialize(userStatus);
            }
            
            repository.User.Update(user);
            repository.User.Save();
        }

        public void AddUserStatus(int userId, UserStatus status)
        {
            User user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;
            List<UserStatus> userStatus = JsonSerializer.Deserialize<List<UserStatus>>(user.Status)!;
            if (status == UserStatus.Ban)
            {
                userStatus.Add(UserStatus.Ban);
                userStatus = userStatus.Where(x => x == UserStatus.Ban).ToList(); 
                user.Status = JsonSerializer.Serialize(userStatus);
            }
            else
            {
                
                userStatus.Add(status);
                user.Status = JsonSerializer.Serialize(userStatus);
            }
            
            repository.User.Update(user);
            repository.User.Save();
        }

        public User Profile(int userId)
        {
            return repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;
        }

        public void Update(int userId, UpdateUserRequest request)
        {
            User? user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault();
            repository.User.Update(mapper.Map(request, user!));
            repository.User.Save();
        }
    }
}
