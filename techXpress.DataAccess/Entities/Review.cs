using System;

namespace techXpress.DataAccess.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign keys
        public Guid UserId { get; set; }
        public int ProductId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
}