namespace techXpress.UI.VMs.Products
{
    public class PagedProductListVM
    {
        public IEnumerable<ProductVM> Products { get; set; } = new List<ProductVM>();
        public int TotalCount { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
