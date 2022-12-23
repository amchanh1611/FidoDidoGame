using AutoMapper;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Modules.FidoDidos.Response;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace FidoDidoGame.Modules.FidoDidos.Service
{
    public interface IFidoDidoService
    {
        void CreateFido(CreateFidoRequest request);
        void CreateMultiDido(List<string> didoNames);
        void CreateFidoDido(CreateFidoDidoRequest request);
        void UpdatePercentFido(UpdatePercentFidoRequest request);
        FidoResponse Fido(int userId);
    }
    public class FidoDidoService : IFidoDidoService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly IBackgroundJobClient hangfire;

        public FidoDidoService(IRepository repository, IMapper mapper, IBackgroundJobClient hangfire, IRedisService redis)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.hangfire = hangfire;
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
            repository.Fido.Create(dido);
            repository.Fido.Save();
        }

        public FidoResponse Fido(int userId)
        {
            User? user = repository.User.FindByCondition(x => x.Id == userId).Include(x => x.UserStatus)!.Include(x=>x.Fido).FirstOrDefault();

            List<Fido> fidos  = repository.Fido.FindAll().OrderBy(x=>x.PercentRand).ToList();
            Random rand = new();
            int point = rand.Next(0, 100);
            foreach (Fido fido in fidos)
            {
                if (point <= fido.Percent)
                {
                    user!.FidoId = fido.Id;
                    repository.User.Update(user);
                    repository.User.Save();
                    break;
                }
            }

            return new FidoResponse(userId,user!.Fido!.Name, user!.UserStatus!.Select(x => x.StatusCode).ToList());
        }

        public void UpdatePercentFido(UpdatePercentFidoRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
