using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.DataAccess.Abstraction
{
    public interface IRepository <T> where T : class
    {
        IEnumerable<T> GetAll(params string[]? Includes);
        IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
