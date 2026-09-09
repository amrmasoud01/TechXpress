 using techXpress.Services.DTOs.Products;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.ActionRequests
{
    public class UpdateProductActionRequest
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        public IFormFile? Image { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public int CategoryId { get; set; }
    }

    public static class UpdateProductActionRequestMapping
    {
        public static ProductDTO ToDto(this UpdateProductActionRequest request)
        {
            return new ProductDTO
            {
                Id = request.Id,
                Name = request.Name,
                CategoryId = request.CategoryId,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Description = request.Description
            };
        }

        public static UpdateProductActionRequest ToActionRequest(this ProductDTO product)
        {
            return new UpdateProductActionRequest
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Description = product.Description,
                ImageUrl = product.Image
            };
        }


    }
}
