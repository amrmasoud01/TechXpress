using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techXpress.DataAccess.Entities;
using techXpress.Services.DTOs.CategoryDTOs;

namespace techXpress.Services.DTOs.Products
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Image { get; set; }

        public int CategoryId { get; set; }
        public int SellerId { get; set; }

        public CategoryDTO CategoryDTO { get; set; }
        public ICollection<ProductReviewDTO> Reviews { get; set; }
    }

    public static class ProductDetailsDtoExtensions
    {
        public static ProductDetailsDto ToProductDetailsDto(this Product product)
        {
            return new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Image = product.Image,
                CategoryId = product.CategoryId,
                SellerId = product.SellerId,
                CategoryDTO = product.Category.ToDTO(),
                Reviews = product.Reviews.Select(r => r.ToProductReviewDTO()).ToList()
            };
        }
    }
}
