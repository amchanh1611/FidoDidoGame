using AutoMapper;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Modules.FidoDidos.Response;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Persistents.Repositories;
using Hangfire;

namespace FidoDidoGame.Modules.FidoDidos.Service
{
    public interface IFidoDidoService
    {
        void CreateMultiFido(List<string> fidoNames);
        void CreateMultiDido(List<string> didoNames);
        void CreateFidoDido(CreateFidoDidoRequest request);
        FidoResponse Fido(int userId);
    }
    public class FidoDidoService : IFidoDidoService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly IBackgroundJobClient hangfire;

        public FidoDidoService(IRepository repository, IMapper mapper, IBackgroundJobClient hangfire)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.hangfire = hangfire;
        }

        public void CreateFidoDido(CreateFidoDidoRequest request)
        {
            repository.FidoDido.Create(mapper.Map<CreateFidoDidoRequest, FidoDido>(request));
            repository.Save();
        }

        public void CreateMultiDido(List<string> didoNames)
        {
            List<Dido> didos = didoNames.Select(x => new Dido { Name = x }).ToList();
            repository.Dido.CreateMulti(didos);
            repository.Save();
        }

        public void CreateMultiFido(List<string> fidoNames)
        {
            List<Fido> didos = fidoNames.Select(x => new Fido { Name = x }).ToList();
            repository.Fido.CreateMulti(didos);
            repository.Save();
        }

        public FidoResponse Fido(int userId)
        {
            User? user = repository.User.FindByCondition(x => x.Id == userId).FirstOrDefault();
            List<Fido> fidos = repository.Fido.FindAll().OrderBy(x=>x.Percent).ToList();

            return new FidoResponse();
        }
    }
}
