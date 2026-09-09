using techXpress.UI.VMs.Category;

namespace techXpress.UI.VMs.Products
{
    public class ProductsWithCategoriesVM
    {
        public IEnumerable<ProductVM> Products { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
        public ProductFilterVM ProductFilter { get; set; }
    }
}
