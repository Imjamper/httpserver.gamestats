using GL.HttpServer.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Database;
using GL.HttpServer.Entities;
using LiteDB;

namespace GL.HttpServer.Managers
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        protected readonly ILiteUnitOfWork UnitOfWork;
        public Repository(ILiteUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void Add(IList<TEntity> items) 
        {
            foreach (var entity in items)
            {
                var entityCollection = UnitOfWork.Collection<TEntity>();
                entityCollection.Insert(entity);
            }
        }

        public void Add(TEntity item)
        {
            Collection.Insert(item);
        }

        public void Delete(TEntity item)
        {
            Collection.Delete(new BsonValue(item.Id));
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            Collection.Delete(expression);
        }

        public void DeleteAll()
        {
            UnitOfWork.DeleteAll<TEntity>();
        }

        public IList<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return Collection.Find(expression).ToList();
        }

        public IList<TEntity> FindAll()
        {
            return Collection.FindAll().ToList();
        }

        public TEntity LoadOrNull(int id)
        {
            return Collection.FindById(id);
        }

        public void Update(TEntity item)
        {
            Collection.Update(item);
        }

        public LiteCollection<TEntity> Collection => UnitOfWork.Collection<TEntity>();
    }
}
