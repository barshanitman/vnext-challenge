using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vnext.Function.Entities;

namespace Vnext.Function
{
    public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : class
    {
        public readonly DeviceContext _context;

        public readonly DbSet<TEntity> _entities;

        public GenericRepo(DeviceContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }


        public TEntity Add(TEntity entity)
        {

            _entities.Add(entity);
            return entity;
        }

        public bool EntityExists(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {

            return await _entities.ToListAsync();
        }

        public Task<TEntity> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveEntity(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChanges()
        {

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;

        }

        public TEntity UpdateEntity(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }


}