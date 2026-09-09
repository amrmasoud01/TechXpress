using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.Products;
using techXpress.DataAccess.Abstraction;
using techXpress.DataAccess.Entities;
using techXpress.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.Services.Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(p => p.Id == id);

            if(product != null) 
                return product.ToDto();

            return null;
        }

        public async Task<ProductDetailsDto?> GetProductDetailsAsync(int id)
        {
            Product? product = await _unitOfWork.ProductRepository.GetProductWithDetailsAsync(id);
            if (product != null)
            {
                return product.ToProductDetailsDto();
            }
            return null;
        }

        public async Task CreateProductAsync(ProductDTO productDTO)
        {
            Product product = productDTO.ToProduct();
            await _unitOfWork.ProductRepository.CreateAsync(product);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<ProductDTO> GetAll(params string[]? Includes) 
        {
            var productDTOs = _unitOfWork.ProductRepository.GetAll(Includes).Select(p => p.ToDto());
            return productDTOs;
        }

        public async Task UpdateProductAsync(ProductDTO productDTO)
        {
            _unitOfWork.ProductRepository.Update(productDTO.ToProduct());
            await _unitOfWork.SaveAsync();
        }

        public async Task AddReviewAsync(int productId, ProductReviewDTO reviewDTO)
        {
            await _unitOfWork.ProductRepository.AddReviewAsync(productId, reviewDTO.ToReview());
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<ProductDTO> GetProductsWhere(ProductQueryDTO productQuery)
        {
            IEnumerable<ProductDTO> products = _unitOfWork.ProductRepository.GetAll("Reviews")
                .Where(p => (productQuery.CategoryId == null || p.CategoryId == productQuery.CategoryId) &&
                            (string.IsNullOrEmpty(productQuery.SearchTerm) || p.Name.Contains(productQuery.SearchTerm)))
                .Select(p => p.ToDto());

            if (productQuery.SortBy == "PriceAsc")
            {
                products = products.OrderBy(p => p.Price);
            }
            else if (productQuery.SortBy == "PriceDesc")
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else if (productQuery.SortBy == "Name")
            {
                products = products.OrderBy(p => p.Name);
            }

            return products;
        }

        public IEnumerable<ProductDTO> GetProductsByCategory(int categoryId)
        {
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetProductsByCategory(categoryId);
            return products.Select(p => p.ToDto());
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(p => p.Id == id);
                if (product == null)
                {
                    return;
                }

                _unitOfWork.ProductRepository.Delete(product);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("Product deletion failed", ex);
            }
        }
    }
}
