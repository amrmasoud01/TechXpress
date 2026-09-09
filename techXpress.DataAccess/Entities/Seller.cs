using System;
using System.Collections.Generic;

namespace techXpress.DataAccess.Entities
{
    public class Seller
    {
        public int SellerId { get; set; }
        public string? StoreName { get; set; }
        public string SellerName { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}