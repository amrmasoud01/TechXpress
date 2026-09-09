using techXpress.Services.DTOs.Products;

namespace techXpress.UI.VMs.Products
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public int StockQuantity { get; set; }
        public IEnumerable<ProductReviewVM> Reviews { get; set; } = new List<ProductReviewVM>();
    }

    public static class ProductDetailsMapping
    {
        public static ProductDetailsVM ToVM(this ProductDetailsDto product)
        {
            return new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.Image,
                CategoryName = product.CategoryDTO.Name,
                StockQuantity = product.StockQuantity,
                Price = product.Price,
                Reviews = product.Reviews.Select(r => new ProductReviewVM
                {
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    Rating = r.Rating,
                    UserName = r.UserName,
                    UserId = r.UserId
                }).ToList()
            };
        }
    }
}
