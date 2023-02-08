using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Vnext.Function
{

    public interface IGenericRepo<TEntity>
    {

        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetById(int Id);

        TEntity Add(TEntity entity);

        TEntity UpdateEntity(TEntity entity);

        Task<bool> SaveChanges();

        Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);
        Task<bool> RemoveEntity(int id);

        public bool EntityExists(int Id);


    }



}