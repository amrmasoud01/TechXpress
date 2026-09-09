namespace techXpress.UI.VMs.WishList
{
    public class WishListItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
