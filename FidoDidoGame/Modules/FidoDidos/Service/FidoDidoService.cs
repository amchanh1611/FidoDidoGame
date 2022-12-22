using AutoMapper;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Persistents.Repositories;

namespace FidoDidoGame.Modules.FidoDidos.Service
{
    public interface IFidoDidoService
    {
        void CreateMultiFido(List<string> fidoNames);
        void CreateMultiDido(List<string> didoNames);
        void CreateFidoDido(CreateFidoDidoRequest request);
    }
    public class FidoDidoService : IFidoDidoService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public FidoDidoService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
    }
}
