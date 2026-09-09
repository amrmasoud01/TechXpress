using techXpress.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.DataAccess.Abstraction
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        IEnumerable<Product> GetProductsBySeller(int sellerId);
        Task<Product?> GetProductWithDetailsAsync(int productId);
        Task AddReviewAsync(int id, Review review);
    }
}
