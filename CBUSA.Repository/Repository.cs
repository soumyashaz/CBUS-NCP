using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected readonly DbContext _Context;

        public Repository(DbContext Context)
        {
            _Context = Context;
        }


        public TEntity Get(long Id)
        {
            return _Context.Set<TEntity>().Find(Id);
        }




        public IEnumerable<TEntity> GetAll()
        {
            return _Context.Set<TEntity>().AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> Find(Func<TEntity, bool> Predicate)
        {
            // return _Context.Set<TEntity>().Where(Predicate).ToList();
            IQueryable<TEntity> query = _Context.Set<TEntity>();
            return query.Where(Predicate).ToList();

        }

        public IEnumerable<TEntity> Search(System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate,

           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null)
        {
            IQueryable<TEntity> query = _Context.Set<TEntity>();
            if (OrderBy != null)
            {
                query = query.Where(Predicate);
                return OrderBy(query).ToList();
            }
            else
            {
                return query.Where(Predicate).ToList();
            }
        }

        public async Task<IEnumerable<TEntity>> SearchAsyn(System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null)
        {
            IQueryable<TEntity> query = _Context.Set<TEntity>();
            if (OrderBy != null)
            {
                query = query.Where(Predicate);
                return await OrderBy(query).ToListAsync();
            }
            else
            {
                return await query.Where(Predicate).ToListAsync();
            }
        }

        public IEnumerable<TEntity> SearchWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _Context.Set<TEntity>();
            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }
            return query.Where(Predicate).ToList();
        }

        public void Add(TEntity Entity)
        {
            try
            {
                _Context.Configuration.AutoDetectChangesEnabled = false;
                _Context.Set<TEntity>().Add(Entity);
            }
            finally
            {
                _Context.Configuration.AutoDetectChangesEnabled = true;
            }
                    
        }

        public void AddRange(IEnumerable<TEntity> Entity)
        {
            try
            {
                _Context.Configuration.AutoDetectChangesEnabled = false;
                _Context.Set<TEntity>().AddRange(Entity);
            }
            finally
            {
                _Context.Configuration.AutoDetectChangesEnabled = true;
            }
           
        }

        public void Remove(TEntity Entity)
        {
            //  _Context.Set<TEntity>().Add(Entity);
            _Context.Entry(Entity).State = System.Data.Entity.EntityState.Deleted;
        }

        public void RemoveRange(IEnumerable<TEntity> Entity)
        {
            _Context.Set<TEntity>().RemoveRange(Entity);
        }

        public void Update(TEntity Entity)
        {
            //  _Context.Entry(Entity).de

            _Context.Entry(Entity).State = System.Data.Entity.EntityState.Modified;
            _Context.SaveChanges();

        }

        public void UpdateAsync(TEntity Entity)
        {
            //  _Context.Entry(Entity).de
            
            _Context.Entry(Entity).State = System.Data.Entity.EntityState.Modified;
            //_Context.SaveChangesAsync();

        }

        public void Detach(TEntity Entity)
        {
            //  _Context.Entry(Entity).de

            _Context.Entry(Entity).State = System.Data.Entity.EntityState.Detached;

        }

        
    }
}
