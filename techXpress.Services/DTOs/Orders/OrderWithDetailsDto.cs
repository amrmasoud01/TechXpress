using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techXpress.DataAccess.Entities.Enums;
using techXpress.DataAccess.Entities;

namespace techXpress.Services.DTOs.Orders
{
    public class OrderWithDetailsDto
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

        public Guid UserId { get; set; }
        public string? PaymentId { get; set; }
        public string? SessionId { get; set; }
        public int? CouponId { get; set; }

        public User User { get; set; }
        public Payment? Payment { get; set; }
        public Coupon? Coupon { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public static class OrderWithDetailsDtoExtenstions
    {
        public static OrderWithDetailsDto ToOrderWithDetailsDto(this Order order)
        {
            return new OrderWithDetailsDto
            {
                OrderId = order.OrderId,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                ShippingDate = order.ShippingDate,
                Carrier = order.Carrier,
                TrackingNumber = order.TrackingNumber,
                Address = order.Address,
                City = order.City,
                RecipientPhoneNumber = order.RecipientPhoneNumber,
                UserId = order.UserId,
                PaymentId = order.PaymentId,
                SessionId = order.SessionId,
                CouponId = order.CouponId,
                User = order.User,
                Payment = order.Payment,
                Coupon = order.Coupon,
                OrderDetails = order.OrderDetails,
            };
        }
    }
}
