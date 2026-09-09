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
    public class ProductRepository : Repository<Product> , IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
            :base(db)
        {
            _db = db;
        }

        public async Task AddReviewAsync(int id,Review review)
        {
            Product? productToUpdate = await _db.Products
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(productToUpdate == null)
            {
                throw new ArgumentException("Product not found.", nameof(id));
            }

            if (productToUpdate.Reviews.Any(existingReview => existingReview.UserId == review.UserId))
            {
                throw new InvalidOperationException("You have already reviewed this product.");
            }

            productToUpdate.Reviews.Add(review);   
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Invalid category ID.", nameof(categoryId));

            try
            {
                return _db.Products.Where(p => p.CategoryId == categoryId).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving products for category ID {categoryId}.", ex);
            }
        }

        public IEnumerable<Product> GetProductsBySeller(int sellerId)
        {
            if (sellerId <= 0)
                throw new ArgumentException("Invalid seller ID.", nameof(sellerId));

            try
            {
                return _db.Products.Where(p => p.SellerId == sellerId).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving products for seller ID {sellerId}.", ex);
            }
        }

        public async Task<Product?> GetProductWithDetailsAsync(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product ID.", nameof(productId));

            try
            {
                return await _db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Seller)
                    .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                    .FirstOrDefaultAsync(p => p.Id == productId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving product details for product ID {productId}.", ex);
            }
        }

    }
}
