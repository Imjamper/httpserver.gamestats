using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Database;
using GL.HttpServer.Entities;
using LiteDB;

namespace GL.HttpServer.Managers
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        void Add(IList<TEntity> items);
        void Add(TEntity item);
        void DeleteAll();
        void Update(TEntity item);
        void Delete(TEntity item);
        void Delete(Expression<Func<TEntity, bool>> expression);
        IList<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        TEntity FindOne(Expression<Func<TEntity, bool>> expression);
        IList<TEntity> FindAll();
        TEntity LoadOrNull(int id);
        LiteCollection<TEntity> Collection { get; }
    }
}
