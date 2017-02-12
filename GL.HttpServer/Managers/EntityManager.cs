using GL.HttpServer.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Database;
using GL.HttpServer.Models;
using LiteDB;

namespace GL.HttpServer.Managers
{
    public class EntityManager<TEntity> : IEntityManager<TEntity> where TEntity : IEntity
    {
        public static EntityManager<TEntity> Instance => new EntityManager<TEntity>();

        public void Add(IList<TEntity> items) 
        {
            using (var database = GetDatabase())
            {
                foreach (var entity in items)
                {
                    var entityCollection = database.GetCollection<TEntity>(typeof(TEntity).Name);
                    entityCollection.Insert(entity);
                }
            }
        }

        public void Add(TEntity item)
        {
            using (var database = GetDatabase())
            {
                var entityCollection = database.GetCollection<TEntity>(typeof(TEntity).Name);
                entityCollection.Insert(item);
            }
        }

        public void Delete(TEntity item)
        {
            using (var database = GetDatabase())
            {
                var entityCollection = database.GetCollection<TEntity>(typeof(TEntity).Name);
                entityCollection.Delete(new BsonValue(item.Id));
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            using (var database = GetDatabase())
            {
                var entityCollection = database.GetCollection<TEntity>(typeof(TEntity).Name);
                entityCollection.Delete(expression);
            }
        }

        public void DeleteAll()
        {
            using (var database = GetDatabase())
            {
                database.DropCollection(typeof(TEntity).Name);
            }
        }

        public IList<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            using (var database = GetDatabase())
            {
                var entityCollection = database.GetCollection<TEntity>(typeof(TEntity).Name);
                return entityCollection.Find(expression).ToList();
            }
        }

        public IList<TEntity> FindAll()
        {
            using (var database = GetDatabase())
            {
                var entityCollection = database.GetCollection<TEntity>(typeof(TEntity).Name);
                return entityCollection.FindAll().ToList();
            }
        }

        public TEntity LoadOrNull(int id)
        {
            using (var database = GetDatabase())
            {
                var entityCollection = database.GetCollection<TEntity>(typeof(TEntity).Name);
                return entityCollection.FindById(id);
            }
        }

        public DbContext GetDatabase()
        {
            return new DbContext(ServerEnviroment.ConnectionString);
        }
    }
}
