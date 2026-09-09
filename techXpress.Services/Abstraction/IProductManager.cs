using techXpress.Services.DTOs.Products;
using techXpress.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.Services.Abstraction
{
    public interface IProductManager
    {
        Task<ProductDTO?> GetByIdAsync(int id);
        Task CreateProductAsync(ProductDTO productDTO);
        Task UpdateProductAsync(ProductDTO productDTO);
        IEnumerable<ProductDTO> GetAll(params string[]? Includes);
        IEnumerable<ProductDTO> GetProductsWhere(ProductQueryDTO productQuery);
        IEnumerable<ProductDTO> GetProductsByCategory(int categoryId);
        Task<ProductDetailsDto?> GetProductDetailsAsync(int id);
        Task DeleteProductAsync(int id);
        Task AddReviewAsync(int productId, ProductReviewDTO reviewDTO);
    }
}
