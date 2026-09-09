using techXpress.Services.DTOs.Orders;

namespace techXpress.UI.VMs.Account
{
    public class UserProfileVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<GetAllOrdersDto> OrderHistory { get; set; }
    }
}
