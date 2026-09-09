using techXpress.Services.DTOs.Products;
using techXpress.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.ActionRequests
{
    public class CreateProductActionRequest
    {
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0 , int.MaxValue)]
        public int StockQuantity { get; set; }
        [Required]
        public IFormFile ImageUrl { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public int CategoryId { get; set; }
    }

    public static class CreateProductActionRequestMapping
    {
        public static ProductDTO ToDto(this CreateProductActionRequest request)
        {
            return new ProductDTO
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Description = request.Description
            };
        }
    }

}
