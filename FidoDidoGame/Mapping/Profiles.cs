using AutoMapper;
using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Persistents.Repositories;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Rank.Request;
using FidoDidoGame.Persistents.Redis.Entities;
using FidoDidoGame.Modules.Rank.Response;
using FidoDidoGame.Modules.Rank.Entities;
using BC = BCrypt.Net.BCrypt;

namespace FidoDidoGame.Mapping
{
    public class Profiles : Profile
    {
        public Profiles(IRepository repository)
        {
            //Users
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest=>dest.Password, opt=>opt.MapFrom(src=>BC.HashPassword(src.Password)));
            CreateMap<UpdateUserRequest, User>();

            //FidoDido
            CreateMap<CreateFidoDidoRequest, FidoDido>()
                .ForMember(dest => dest.PercentRand, opt => opt.MapFrom(src => repository.FidoDido.FindByCondition(x => x.FidoId == src.FidoId).Sum(x => x.Percent) + src.Percent));
            CreateMap<CreateFidoRequest, Fido>()
                .ForMember(dest => dest.PercentRand, opt => opt.MapFrom(src => repository.Fido.FindAll().Sum(x=>x.Percent)+src.Percent));

            //Rank
            long miliDateStatic = (long)(new DateTime(2100, 12, 31, 23, 59, 59)).Subtract(DateTime.Now).TotalMilliseconds;

            CreateMap<CreatePointOfRoundRequest, PointOfRound>();
            CreateMap<CreatePointDetailRequest, PointDetail>();
            CreateMap<UpdateRank, CreatePointOfRoundRequest>();
            CreateMap<UpdateRank, UpdatePointOfRoundRequest>();
            CreateMap<UserRankOfRoundIn, RankingResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Now.AddMilliseconds(miliDateStatic - src.DateMiliSecond).ToLocalTime()));

            //Event
            CreateMap<CreateEventRequest, Event>();
            CreateMap<UpdateEventRequest, Event>();

            //Reward
            CreateMap<CreateRewardRequest, Reward>();

            //Role
            CreateMap<CreateRoleRequest, Role>();
            
        }
    }
}
