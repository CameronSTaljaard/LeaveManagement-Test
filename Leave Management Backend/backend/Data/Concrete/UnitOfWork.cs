using backend.Data.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace backend.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _dbContext = new DataContext();
        private bool disposed = false;

        private GenericRepository<Request> _requestRepository;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Resolvement> _resolvementRepository;

        private Dictionary<Type, object> _genericRepositoryDict = new Dictionary<Type, object>();

        public object? GetRepository<T>()
        {
            if (_genericRepositoryDict.Count == 0)
            {
                GetType()
                    .GetProperties()
                    .Where(x =>
                        x.PropertyType.IsGenericType &&
                        x.PropertyType.GetGenericTypeDefinition() == typeof(GenericRepository<>)
                    ).ToList()
                    .ForEach(prop =>
                        _genericRepositoryDict.Add(
                            prop.PropertyType.GetGenericArguments()[0], //Key
                            prop.GetValue(this) //value
                        )
                    );
            }
            return _genericRepositoryDict[typeof(T)];
        }

        R Init<T, R>(ref R repo)
            where T : class
            where R : GenericRepository<T>, new()
        {
            if (repo == null)
            {
                repo = new R();
                repo.Init(_dbContext);
            }
            return repo;
        }

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DatabaseFacade Database
        { get => _dbContext.Database; }


        public DataContext DbContext
        { get => _dbContext; }

        public GenericRepository<Request> RequestRepository
        { get => Init<Request, GenericRepository<Request>>(ref _requestRepository); }
        public GenericRepository<User> UserRepository
        { get => Init<User, GenericRepository<User>>(ref _userRepository); }
        public GenericRepository<Resolvement> ResolvementRepository
        { get => Init<Resolvement, GenericRepository<Resolvement>>(ref _resolvementRepository); }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }
    }
}
