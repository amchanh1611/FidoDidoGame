using AutoMapper;
using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Users.Request;

namespace FidoDidoGame.Mapping
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            //Users
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();

            //FidoDido
            CreateMap<CreateFidoDidoRequest, FidoDido>();
        }
    }
}
