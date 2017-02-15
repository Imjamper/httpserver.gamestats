using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Entities;
using GL.HttpServer.Managers;
using LiteDB;

namespace GL.HttpServer.Database
{
    public interface ILiteUnitOfWork : IDisposable
    {
        void Transaction(Action<ILiteUnitOfWork> body);

        bool TransactionSaveChanges(Action<ILiteUnitOfWork> body);

        IRepository<TEntity> Repository<TEntity>() where TEntity : IEntity;

        LiteCollection<TEntity> Collection<TEntity>() where TEntity : IEntity;

        void DeleteAll<TEntity>();
    }
}
