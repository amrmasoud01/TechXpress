using techXpress.Services.DTOs.Products;

using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.VMs.Products
{
    public class ProductReviewVM
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }    
    }
    public static class ProductReviewVMExtensions
    {
        public static ProductReviewDTO ToDto(this ProductReviewVM productReviewDTO)
        {
            return new ProductReviewDTO
            {
                Comment = productReviewDTO.Comment,
                CreatedAt = productReviewDTO.CreatedAt,
                Rating = productReviewDTO.Rating,
                UserId = productReviewDTO.UserId,
                UserName = productReviewDTO.UserName,
            };
        }
    }
}
