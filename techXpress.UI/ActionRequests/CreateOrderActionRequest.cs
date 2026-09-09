using System.ComponentModel.DataAnnotations;
using techXpress.Services.DTOs.Orders;

namespace techXpress.UI.ActionRequests
{
    public class CreateOrderActionRequest
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string RecipientPhoneNumber { get; set; }
        public int? CouponId { get; set; }
    }

    public static class CreateOrderActionRequestExtenstions
    {
        public static CreateOrderDTO ToDto(this CreateOrderActionRequest request)
        {
            return new CreateOrderDTO
            {
                Address = request.Address,
                City = request.City,
                RecipientPhoneNumber = request.RecipientPhoneNumber, 
                CouponId = request.CouponId,
                UserId = request.UserId
            };
        }
    }
}
