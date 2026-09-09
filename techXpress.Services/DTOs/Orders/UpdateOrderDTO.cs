using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techXpress.DataAccess.Entities;
using techXpress.DataAccess.Entities.Enums;

namespace techXpress.Services.DTOs.Orders
{
    public class UpdateOrderDTO
    {
        public int Id { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public string? PaymentId { get; set; }
        public string? SessionId { get; set; }
        public string? Carrier { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public int? CouponId { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? ShippingDate { get; set; }
    }
}
