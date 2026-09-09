namespace techXpress.UI.VMs.WishList
{
    public class WishListVM
    {
        public List<WishListItemVM> WishListItems { get; set; } = new();
        public int Count => WishListItems.Count;
    }
}
