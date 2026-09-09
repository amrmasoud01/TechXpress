using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techXpress.DataAccess.Entities;

namespace techXpress.Services.DTOs.Orders
{
    public class GetAllOrdersDto
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
        public string UserEmail { get; set; }
    }

    public static class GetAllOrdersExtenstions
    {
        public static GetAllOrdersDto GetAllOrdersDto(this Order order)
        {
            return new GetAllOrdersDto
            {
                OrderId = order.OrderId,
                OrderStatus = order.OrderStatus.ToString(),
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
                UserEmail = order.User.Email
            };
        }
    }
}
