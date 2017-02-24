using System;
using System.Collections.Generic;
using GL.HttpServer.Entities;
using GL.HttpServer.Repository;
using GL.HttpServer.Types;
using LiteDB;

namespace GL.HttpServer.Database
{
    public class LiteUnitOfWork : ILiteUnitOfWork
    {
        private readonly LiteDatabase _database;
        private Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public LiteUnitOfWork()
        {
            _database = LiteDb.ReadWrite;
        }

        public void Dispose()
        {
            _repositories.Clear();
            _repositories = null;
            GC.SuppressFinalize(this);
        }

        public void Transaction(Action<ILiteUnitOfWork> body)
        {
            using (var transaction = _database.BeginTrans())
            {
                try
                {
                    body.Invoke(this);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public bool TransactionSaveChanges(Action<ILiteUnitOfWork> body)
        {
            using (var transaction = _database.BeginTrans())
            {
                try
                {
                    body.Invoke(this);
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : IEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as Repository<TEntity>;
            }
            var repository = InstanceActivator.CreateInstance(typeof(Repository<TEntity>), this) as Repository<TEntity>;
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public LiteCollection<TEntity> Collection<TEntity>() where TEntity : IEntity
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public void DeleteAll<TEntity>()
        {
            _database.DropCollection(typeof(TEntity).Name);
        }
    }

    public class UnitOfWork : LiteUnitOfWork
    {
    }
}
