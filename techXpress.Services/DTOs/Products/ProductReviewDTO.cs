using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techXpress.DataAccess.Entities;

namespace techXpress.Services.DTOs.Products
{
    public class ProductReviewDTO
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }

    }

    public static class ProductReviewDTOExtensions
    {
        public static Review ToReview(this ProductReviewDTO reviewDTO)
        {
            return new Review
            {
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                CreatedAt = reviewDTO.CreatedAt,
                UserId = reviewDTO.UserId,
                ProductId = reviewDTO.ProductId
            };
        }
        public static ProductReviewDTO ToProductReviewDTO(this Review review)
        {
            return new ProductReviewDTO
            {
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UserId = review.UserId,
                ProductId = review.ProductId,
                UserName = review.User?.UserName ?? "Unknown"
            };
        }
    }
}
