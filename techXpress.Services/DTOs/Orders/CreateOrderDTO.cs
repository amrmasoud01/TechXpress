using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using techXpress.DataAccess.Entities;

namespace techXpress.Services.DTOs.Orders
{
    public class CreateOrderDTO
    {
        public Guid UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public int? CouponId { get; set; }
        [ValidateNever]
        public decimal TotalAmount { get; set; }
        public Dictionary<int,int> ProductQuantities { get; set; }
    }

    public static class OrderDTOExtenstions
    {
        public static Order ToOrder(this CreateOrderDTO orderDto)
        {
            return new Order
            {
                Address = orderDto.Address,
                City = orderDto.City,
                RecipientPhoneNumber = orderDto.RecipientPhoneNumber,
                UserId = orderDto.UserId,
                CouponId = orderDto.CouponId
            };
        }
    }
}
