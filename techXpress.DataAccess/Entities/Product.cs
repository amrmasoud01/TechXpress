using System;
using System.Collections.Generic;

namespace techXpress.DataAccess.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Image { get; set; }

        // Foreign keys
        public int CategoryId { get; set; }
        public int SellerId { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public Seller Seller { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}