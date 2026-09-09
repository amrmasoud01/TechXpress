using techXpress.DataAccess.Entities.Enums;
using System;
using System.Collections.Generic;

namespace techXpress.DataAccess.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string RecipientPhoneNumber { get; set; }

        // Foreign keys
        public Guid UserId { get; set; }
        public string? PaymentId { get; set; }
        public string? SessionId { get; set; }
        public int? CouponId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Payment? Payment { get; set; }
        public Coupon? Coupon { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}