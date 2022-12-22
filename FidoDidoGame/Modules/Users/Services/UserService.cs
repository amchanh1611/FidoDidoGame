using AutoMapper;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Persistents.Repositories;

namespace FidoDidoGame.Modules.Users.Services
{
    public interface IUserService
    {
        void Create(CreateUserRequest request);
        void Update(int userId, UpdateUserRequest request);
        void CreateStatus(List<string> statusName);
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
            repository.User.Create(mapper.Map<CreateUserRequest, User>(request));
            repository.Save();
        }

        public void CreateStatus(List<string> statusName)
        {
            List<Status> status = statusName.Select(x => new Status { Name = x }).ToList();
            repository.Status.CreateMulti(status);
            repository.Save();
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
}
