using techXpress.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techXpress.Services.DTOs.Products
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public double AverageRating { get; set; } = 0;
    }

    public static class ProductDTOMapping 
    {
        public static ProductDTO ToDto(this Product product)
        {
            double avgRating = 0;
            if (product.Reviews != null && product.Reviews.Any())
            {
                avgRating = Math.Round(product.Reviews.Average(r => r.Rating), 1);
            }

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Image = product.Image,
                CategoryId = product.CategoryId,
                AverageRating = avgRating
            };
        }

        public static Product ToProduct(this ProductDTO productDTO)
        {
            return new Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                StockQuantity = productDTO.StockQuantity,
                Image = productDTO.Image,
                CategoryId = productDTO.CategoryId
            };
        }

    }
}
