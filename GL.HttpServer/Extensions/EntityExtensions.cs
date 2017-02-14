using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Dto;
using GL.HttpServer.Entities;

namespace GL.HttpServer.Extensions
{
    public static class EntityExtensions
    {
        public static TDto ToDto<TDto>(this IEntity entity) where TDto : IDto, new()
        {
            var dto = AutoMapper.Mapper.Map<TDto>(entity);
            return dto;
        }

        public static TEntity ToEntity<TEntity>(this IDto dto) where TEntity : IEntity, new()
        {
            var entity = AutoMapper.Mapper.Map<TEntity>(dto);
            return entity;
        }
    }
}
