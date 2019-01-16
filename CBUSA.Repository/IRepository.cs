using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //TEntity Get(Int64 Id);
        IEnumerable<TEntity> GetAll();
        //IEnumerable<TEntity> Find(Func<TEntity, bool> Predicate);


        TEntity Get(Int64 Id);

        IEnumerable<TEntity> Find(Func<TEntity, bool> Predicate);
        IEnumerable<TEntity> Search(System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null);

        Task<IEnumerable<TEntity>> SearchAsyn(System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null);

        IEnumerable<TEntity> SearchWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate, params Expression<Func<TEntity, object>>[] includeExpressions);

        //IEnumerable<TEntity> Find(Func<TEntity, bool> Predicate, params Expression<Func<TEntity, object>>[] includeExpressions);

        void Add(TEntity Entity);
        void AddRange(IEnumerable<TEntity> Entity);
        void Update(TEntity Entity);
        void UpdateAsync(TEntity Entity);
        //void AddRange(IEnumerable<TEntity> Entity);
        void Remove(TEntity Entity);
        void RemoveRange(IEnumerable<TEntity> Entity);
        //void RemoveRange(IEnumerable<TEntity> Entity);

        void Detach(TEntity Entity);
    }
}
