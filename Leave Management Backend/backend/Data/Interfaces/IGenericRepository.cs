using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace backend.Data.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public DbSet<TEntity> DbSet { get; }
        void Delete(object id);
        void Delete(TEntity entity);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string[] includeProperties = null);
        TEntity GetById(object id);
        TEntity Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string[] includeProperties = null);
        void Insert(TEntity entity);
        void InsertMany(TEntity[] entities);
        void Update(TEntity entity);
        int GetCount(Expression<Func<TEntity, bool>> filter = null);

    }
}
