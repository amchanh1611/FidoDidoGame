using FidoDidoGame.Common.RepositoriesBase;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Rank.Entities;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Persistents.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace FidoDidoGame.Persistents.Repositories
{
    public interface IRepository
    {
        IUserRepository User { get; }
        IFidoRepository Fido { get; }
        IDidoRepository Dido { get; }
        IFidoDidoRepository FidoDido { get; }
        IPointOfRoundRepository PointOfRound { get; }
        IPointDetailRepository PointDetail { get; }
        IRewardRepository Reward { get; }
        IEventRepository Event { get; }
        IRoleRepository Role { get; }

        void Save();
        IDbContextTransaction Transaction();
    }

    public interface IEventRepository : IRepositoryBase<Event> { }
    public interface IRewardRepository : IRepositoryBase<Reward> { }
    public interface IUserRepository : IRepositoryBase<User> { }
    public interface IUserStatusRepository : IRepositoryBase<SpecialStatus> { }
    public interface IFidoRepository : IRepositoryBase<Fido> { }
    public interface IDidoRepository : IRepositoryBase<Dido> { }
    public interface IFidoDidoRepository : IRepositoryBase<FidoDido> { }
    public interface IPointOfRoundRepository : IRepositoryBase<PointOfRound> { }
    public interface IPointDetailRepository : IRepositoryBase<PointDetail> { }
    public interface IRoleRepository : IRepositoryBase<Role> { }    

    public class Repository : IRepository
    {
        private readonly AppDbContext context;

        public Repository(AppDbContext context)
        {
            this.context = context;
        }

        private IUserRepository? user;
        private IFidoRepository? fido;
        private IDidoRepository? dido;
        private IFidoDidoRepository? fidoDido;
        private IPointOfRoundRepository? pointOfRound;
        private IPointDetailRepository? pointDetail;
        private IRewardRepository? reward;
        private IEventRepository events;
        private IRoleRepository role;

        public IEventRepository Event
        {
            get
            {
                if(events is null)
                {
                    events = new EventRepository(context);
                }
                return events;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (user is null)
                {
                    user = new UserRepository(context);
                }
                return user;
            }
        }

        public IFidoRepository Fido
        {
            get
            {
                if (fido is null)
                {
                    fido = new FidoRepository(context);
                }
                return fido;
            }
        }

        public IDidoRepository Dido
        {
            get
            {
                if (dido is null)
                {
                    dido = new DidoRepository(context);
                }
                return dido;
            }
        }

        public IFidoDidoRepository FidoDido
        {
            get
            {
                if (fidoDido is null)
                {
                    fidoDido = new FidoDidoRepository(context);
                }
                return fidoDido;
            }
        }

        public IPointOfRoundRepository PointOfRound
        {
            get
            {
                if (pointOfRound is null)
                {
                    pointOfRound = new PointOfRoundRepository(context);
                }
                return pointOfRound;
            }
        }

        public IPointDetailRepository PointDetail
        {
            get
            {
                if (pointDetail is null)
                {
                    pointDetail = new PointDetailRepository(context);
                }
                return pointDetail;
            }
        }

        public IRewardRepository Reward
        {
            get
            {
                if(reward is null)
                {
                    reward = new RewardRepository(context);
                }
                return reward;
            }
        }

        public IRoleRepository Role
        {
            get
            {
                if(role is null)
                {
                    role = new RoleReposiory(context);
                }
                return role;
            }
        }

        public void Save() => context.SaveChanges();

        public IDbContextTransaction Transaction()
        {
            return context.Database.BeginTransaction(); 
        }
    }

    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context) { }
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        { }
    }

    public class FidoRepository : RepositoryBase<Fido>, IFidoRepository
    {
        public FidoRepository(AppDbContext context) : base(context)
        { }
    }
    public class DidoRepository : RepositoryBase<Dido>, IDidoRepository
    {
        public DidoRepository(AppDbContext context) : base(context)
        { }
    }
    public class FidoDidoRepository : RepositoryBase<FidoDido>, IFidoDidoRepository
    {
        public FidoDidoRepository(AppDbContext context) : base(context)
        { }
    }
    public class PointOfRoundRepository : RepositoryBase<PointOfRound>, IPointOfRoundRepository
    {
        public PointOfRoundRepository(AppDbContext context) : base(context)
        { }
    }
    public class PointDetailRepository : RepositoryBase<PointDetail>, IPointDetailRepository
    {
        public PointDetailRepository(AppDbContext context) : base(context) { }
    }
    public class RewardRepository : RepositoryBase<Reward>, IRewardRepository
    {
        public RewardRepository(AppDbContext context) : base(context) { }
    }
    public class RoleReposiory : RepositoryBase<Role>, IRoleRepository
    {
        public RoleReposiory(AppDbContext context) : base(context) { }
    }
}
