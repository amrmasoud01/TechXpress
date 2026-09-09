using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.VMs.ShoppingCart
{
    public class ShoppingCartItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public decimal SubTotal { get; set; }
        public int StockQuantity { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
