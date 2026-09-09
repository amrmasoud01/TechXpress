using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.Services.DTOs.Products
{
    public class ProductQueryDTO
    {
        public string SortBy { get; set; }
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }

    }
}
