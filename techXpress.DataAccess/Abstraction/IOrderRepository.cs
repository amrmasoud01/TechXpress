using techXpress.DataAccess.Entities.Enums;
using techXpress.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.DataAccess.Abstraction
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetOrdersByUser(Guid userId);
        Order? GetOrderWithDetails(int orderId);
        IEnumerable<Order> GetOrdersByStatus(OrderStatus status);
        void UpdateOrderStatus(int orderId, OrderStatus newStatus);
    }
}
