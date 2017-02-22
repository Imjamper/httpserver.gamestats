using System;
using GL.HttpServer.Entities;
using GL.HttpServer.Repository;
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
