using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.VMs.Products
{
    public class ProductFilterVM
    {
        public bool AscendingOrderBasedOnPrice  { get; set; }   
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int Entries { get; set; }
    }
}
