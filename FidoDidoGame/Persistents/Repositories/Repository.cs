using FidoDidoGame.Common.RepositoriesBase;
using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Persistents.Context;

namespace FidoDidoGame.Persistents.Repositories
{
    public interface IRepository
    {
        IUserRepository User { get; }
        IStatusRepository Status { get; }
        IUserStatusRepository UserStatus { get; }
        IFidoRepository Fido { get; }
        IDidoRepository Dido { get; }
        IFidoDidoRepository FidoDido { get; }
        IPointOfDayRepository PointOfDay { get; }
        IPointDetailRepository PointDetail { get; }
    }
    public interface IUserRepository : IRepositoryBase<User> { }
    public interface IStatusRepository : IRepositoryBase<Status> { }
    public interface IUserStatusRepository : IRepositoryBase<UserStatus> { }
    public interface IFidoRepository : IRepositoryBase<Fido> { }
    public interface IDidoRepository : IRepositoryBase<Dido> { }
    public interface IFidoDidoRepository : IRepositoryBase<Modules.FidoDidos.Entities.FidoDido> { }
    public interface IPointOfDayRepository : IRepositoryBase<PointOfDay> { }
    public interface IPointDetailRepository : IRepositoryBase<PointDetail> { }
    public class Repository : IRepository
    {
        private readonly AppDbContext context;

        public Repository(AppDbContext context)
        {
            this.context = context;
        }

        private IUserRepository? user;
        private IStatusRepository? status;
        private IUserStatusRepository? userStatus;
        private IFidoRepository? fido;
        private IDidoRepository? dido;
        private IFidoDidoRepository? fidoDido;
        private IPointOfDayRepository? pointOfDay;
        private IPointDetailRepository? pointDetail;
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

        public IStatusRepository Status
        {
            get
            {
                if (status is null)
                {
                    status = new StatusRepository(context);
                }
                return status;
            }
        }

        public IUserStatusRepository UserStatus
        {
            get
            {
                if (userStatus is null)
                {
                    userStatus = new UserStatusRepository(context);
                }
                return userStatus;
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

        public IPointOfDayRepository PointOfDay
        {
            get
            {
                if (pointOfDay is null)
                {
                    pointOfDay = new PointOfDayRepository(context);
                }
                return pointOfDay;
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
    }
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        { }
    }
    public class StatusRepository : RepositoryBase<Status>, IStatusRepository
    {
        public StatusRepository(AppDbContext context) : base(context)
        { }
    }
    public class UserStatusRepository : RepositoryBase<UserStatus>, IUserStatusRepository
    {
        public UserStatusRepository(AppDbContext context) : base(context)
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
    public class FidoDidoRepository : RepositoryBase<Modules.FidoDidos.Entities.FidoDido>, IFidoDidoRepository
    {
        public FidoDidoRepository(AppDbContext context) : base(context)
        { }
    }
    public class PointOfDayRepository : RepositoryBase<PointOfDay>, IPointOfDayRepository
    {
        public PointOfDayRepository(AppDbContext context) : base(context)
        { }
    }
    public class PointDetailRepository : RepositoryBase<PointDetail>, IPointDetailRepository
    {
        public PointDetailRepository(AppDbContext context) : base(context)
        { }
    }
}
