using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using backend.Data.Interfaces;

namespace backend.Data.Concrete
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DataContext _context;
        internal DbSet<TEntity> _dbSet;
        private string[] _loadedProperties = new string[] { };
        public string[] loadedProperties
        {
            get
            {
                var t = _loadedProperties;
                _loadedProperties = new string[] { };
                return t;
            }
            set => _loadedProperties = value;
        }
        public GenericRepository() { }

        public GenericRepository(DataContext context)
        {
            Init(context);
        }

        public void Init(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public DbSet<TEntity> DbSet { get { return _dbSet; } }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string[] includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //Override parameter combined with input parameter
            var iProps = _loadedProperties
                .Concat(includeProperties ?? new string[] { }).Distinct().ToArray();

            foreach (var includeProperty in iProps)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null) { return orderBy(query).ToList(); }

            return query.ToList();
        }

        public virtual TEntity GetById(object id)
        {
            TEntity? entity = _dbSet.Find(id);

            if (entity == null)
            {
                throw new ArgumentException("ID not found", nameof(GenericRepository<TEntity>));
            }

            return entity;
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void InsertMany(TEntity[] entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public TEntity? Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string[] includeProperties = null)
        {
            return Get(filter, orderBy, includeProperties).FirstOrDefault();
        }

        public int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return Get(filter: filter).Count();
        }
    }
}
