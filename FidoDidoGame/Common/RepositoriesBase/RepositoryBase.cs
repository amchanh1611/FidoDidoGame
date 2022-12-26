using FidoDidoGame.Persistents.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace FidoDidoGame.Common.RepositoriesBase
{
    public interface IRepositoryBase<T>
    {
        T Create(T entity);
        T Update(T entity);
        void UpdateMulti(List<T> entities);
        void Delete(T entity);
        void CreateMulti(List<T> entities);
        void DeleteMulti(List<T> entities);
        void Save();
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IDbContextTransaction Transaction();
    }
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext context;

        public RepositoryBase(AppDbContext context)
        {
            this.context = context;
        }
        public void CreateMulti(List<T> entities) => context.Set<T>().AddRange(entities);

        public T Create(T entity) => context.Set<T>().Add(entity).Entity;

        public void Delete(T entity) => context.Set<T>().Remove(entity);

        public void DeleteMulti(List<T> entities) => context.Set<T>().RemoveRange(entities);

        public IQueryable<T> FindAll() => context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => context.Set<T>().Where(expression);

        public T Update(T entity) => context.Set<T>().Update(entity).Entity;

        public IDbContextTransaction Transaction() => context.Database.BeginTransaction();

        public void Save() => context.SaveChanges();

        public void UpdateMulti(List<T> entities) => context.Set<T>().UpdateRange(entities);
    }

}
