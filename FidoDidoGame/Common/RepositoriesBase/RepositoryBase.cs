using FidoDidoGame.Persistents.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FidoDidoGame.Common.RepositoriesBase
{
    public interface IRepositoryBase<T>
    {
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
        void CreateMulti(List<T> entities);
        void DeleteMulti(List<T> entities);
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
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

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => context.Set<T>().Where(expression).AsNoTracking();

        public T Update(T entity) => context.Set<T>().Update(entity).Entity;
    }

}
