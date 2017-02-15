using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Entities;
using GL.HttpServer.Managers;
using LiteDB;

namespace GL.HttpServer.Database
{
    public class LiteUnitOfWork : ILiteUnitOfWork
    {
        private readonly LiteDatabase _database;
        private Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public LiteUnitOfWork(LiteDatabase context)
        {
            _database = context;
        }

        public void Dispose()
        {
            _database.Dispose();
            _repositories.Clear();
            _repositories = null;
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
            var repository = Activator.CreateInstance(typeof(Repository<TEntity>), this) as Repository<TEntity>;
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

    public class LiteUnitOfWork<TContext> : LiteUnitOfWork where TContext : LiteDatabase, new()
    {
        public LiteUnitOfWork() : base(new TContext())
        {
        }
    }

    public class UnitOfWork : LiteUnitOfWork<LiteDb>
    {
    }
}
