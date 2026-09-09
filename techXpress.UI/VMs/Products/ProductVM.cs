using techXpress.Services.DTOs.Products;

namespace techXpress.UI.VMs.Products
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int StockQuantity { get; set; }
        public double AverageRating { get; set; } = 0;
    }

    public static class ProductDtoToProductVM
    {
        public static ProductVM ToProductVM(this ProductDTO dto)
        {
            return new ProductVM
            {
                Id = dto.Id,
                Price = dto.Price,
                Description = dto.Description,
                Image = dto.Image,
                Name = dto.Name,
                StockQuantity = dto.StockQuantity,
                AverageRating = dto.AverageRating
            };
        }
        public static ProductDTO ToProductDTO(this ProductVM vm)
        {
            return new ProductDTO
            {
                Id = vm.Id,
                Price = vm.Price,
                Description = vm.Description,
                Image = vm.Image,
                Name = vm.Name,
                StockQuantity = vm.StockQuantity,
                AverageRating = vm.AverageRating
            };
        }
    }
}
