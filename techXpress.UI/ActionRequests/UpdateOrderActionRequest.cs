using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using techXpress.DataAccess.Entities;
using techXpress.DataAccess.Entities.Enums;
using techXpress.Services.DTOs.Orders;
using techXpress.Services.DTOs.Products;

namespace techXpress.UI.ActionRequests
{
    public class UpdateOrderActionRequest
    {
        [Display(Name = "Order Status")]
        [Required(ErrorMessage = "Order status is required")]
        public string OrderStatus { get; set; }
        [Display(Name = "Shipping Date")]
        [DataType(DataType.Date)]
        public DateTime? ShippingDate { get; set; }
        public string? Carrier { get; set; }
        [Display(Name = "Tracking Number")]
        public string? TrackingNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Recipient phone number is required")]
        [Display(Name = "Recipient Phone Number")]
        public string RecipientPhoneNumber { get; set; }
        [Display(Name = "Coupon")]
        public int? CouponId { get; set; }
        [ValidateNever]
        public IEnumerable<OrderDetail> OrderDetails { get; set; } 
    }

    public static class UpdateOrderActionRequestExtensions
    {
        public static UpdateOrderActionRequest ToDto(this OrderWithDetailsDto orderDto)
        {
            return new UpdateOrderActionRequest
            {
                OrderStatus = orderDto.OrderStatus.ToString(),
                ShippingDate = orderDto.ShippingDate,
                Carrier = orderDto.Carrier,
                TrackingNumber = orderDto.TrackingNumber,
                Address = orderDto.Address,
                City = orderDto.City,
                RecipientPhoneNumber = orderDto.RecipientPhoneNumber,
                CouponId = orderDto.CouponId,
                OrderDetails = orderDto.OrderDetails,
            };
        }

        public static UpdateOrderDTO ToUpdateOrderDto(this UpdateOrderActionRequest request)
        {
            return new UpdateOrderDTO
            {
                OrderStatus = Enum.TryParse<OrderStatus>(request.OrderStatus, true, out var status) ? status : null,
                ShippingDate = request.ShippingDate,
                Carrier = request.Carrier,
                TrackingNumber = request.TrackingNumber,
                Address = request.Address,
                City = request.City,
                RecipientPhoneNumber = request.RecipientPhoneNumber,
                CouponId = request.CouponId

            };
        }

    }
}
