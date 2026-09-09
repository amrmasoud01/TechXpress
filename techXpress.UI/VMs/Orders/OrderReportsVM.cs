using techXpress.Services.DTOs.Orders;

namespace techXpress.UI.VMs.Orders
{
    public class OrderReportsVM
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CanceledOrders { get; set; }
        public List<MonthlyRevenueData> MonthlyRevenue { get; set; } = new();
        public List<OrderStatusData> OrderStatusDistribution { get; set; } = new();
        public List<GetAllOrdersDto> RecentOrders { get; set; } = new();
    }

    public class MonthlyRevenueData
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public class OrderStatusData
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
