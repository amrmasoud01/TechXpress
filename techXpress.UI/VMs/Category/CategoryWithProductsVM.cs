using techXpress.UI.VMs.Products;

namespace techXpress.UI.VMs.Category
{
    public class CategoryWithProductsVM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
    }
}
