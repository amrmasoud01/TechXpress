using techXpress.DataAccess.Abstraction;
using techXpress.DataAccess.Data;
using techXpress.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public virtual async Task CreateAsync(T entity)
        {
            if(entity is Product p)
            {
                p.SellerId = 1;
            }
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll(params string[]? Includes)
        {
            IQueryable<T> query = _dbSet;
            if(Includes != null)
            {
                foreach (var item in Includes)
                {
                    query = query.Include(item);
                }
            }

            return query;
        }

        public IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public void Update(T entity)
        {
            if (entity is Product p)
            {
                p.SellerId = 1;
            }
            _dbSet.Update(entity);
        }
    }
}
