using backend.Data.Concrete;
using backend.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Data.Interfaces
{
    public interface IUnitOfWork
    {
        public GenericRepository<Request> RequestRepository { get; }
        public GenericRepository<User> UserRepository { get; }
        public GenericRepository<Resolvement> ResolvementRepository { get; }

        public DatabaseFacade Database { get; }
        object? GetRepository<T>();
        void Dispose();
        int Save();
    }
}
