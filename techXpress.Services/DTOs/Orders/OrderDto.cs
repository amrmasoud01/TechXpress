using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techXpress.DataAccess.Entities;
using techXpress.DataAccess.Entities.Enums;

namespace techXpress.Services.DTOs.Orders
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
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
    }

    public static class OrderDtoExtensions
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                SessionId = order.SessionId,
                PaymentId = order.PaymentId,
                OrderStatus = order.OrderStatus.ToString(),
                ShippingDate = order.ShippingDate,
                Carrier = order.Carrier,
                TrackingNumber = order.TrackingNumber,
                Address = order.Address,
                City = order.City,
                RecipientPhoneNumber = order.RecipientPhoneNumber,
                UserId = order.UserId,
                CouponId = order.CouponId,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate
            };
        }
    }
}
