namespace techXpress.UI.VMs.ShoppingCart
{
    public class ShoppingCartVM
    {
        public List<ShoppingCartItemVM> CartItems { get; set; } = new();
        public decimal Total { get; set; }
    }
}
