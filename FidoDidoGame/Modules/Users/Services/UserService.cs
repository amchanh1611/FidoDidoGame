using AutoMapper;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Persistents.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FidoDidoGame.Modules.Users.Services
{
    public interface IUserService
    {
        void Create(CreateUserRequest request);
        void Update(int userId, UpdateUserRequest request);
        void CreateStatus(List<string> statusName);
        void AddUserStatus(int userId, string statusCode);
        void DeleteUserStatus(int userId, string statusCode);
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

        public void AddUserStatus(int userId, string statusCode)
        {
            repository.UserStatus.Create(new UserStatus { UserId = userId, StatusCode = statusCode });
            repository.Status.Save();
        }

        public void Create(CreateUserRequest request)
        {
            using IDbContextTransaction transaction = repository.User.Transaction();
            try
            {
                User user = repository.User.Create(mapper.Map<CreateUserRequest, User>(request));
                repository.User.Save();
                AddUserStatus(user.Id, "normal");
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw new BadRequestException("Create Fail");
            }
        }

        public void CreateStatus(List<string> statusName)
        {
            List<Status> status = statusName.Select(x => new Status { StatusCode = x }).ToList();
            repository.Status.CreateMulti(status);
            repository.Status.Save();
        }

        public void DeleteUserStatus(int userId, string statusCode)
        {
            repository.UserStatus.Delete(new UserStatus { UserId = userId, StatusCode = statusCode });
            repository.Status.Save();
        }

        public User Profile(int userId)
        {
            return repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault()!;
        }

        public void Update(int userId, UpdateUserRequest request)
        {
            User? user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault();
            repository.User.Update(mapper.Map(request, user!));
            repository.Status.Save();
        }
    }
}
