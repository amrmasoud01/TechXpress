using techXpress.DataAccess.Abstraction;
using techXpress.DataAccess.Data;
using techXpress.DataAccess.Entities.Enums;
using techXpress.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace techXpress.DataAccess.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
            : base(db)
        {
            _db = db;
        }

        public IEnumerable<Order> GetOrdersByUser(Guid userId)
        {
            try
            {
                return _db.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Where(o => o.UserId == userId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving orders for user ID {userId}.", ex);
            }
        }

        public Order? GetOrderWithDetails(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.", nameof(orderId));

            try
            {
                return _db.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Include(o => o.Coupon)
                    .FirstOrDefault(o => o.OrderId == orderId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving order details for order ID {orderId}.", ex);
            }
        }

        public IEnumerable<Order> GetOrdersByStatus(OrderStatus status)
        {
            try
            {
                return _db.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderDetails)
                    .Where(o => o.OrderStatus == status)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving orders with status {status}.", ex);
            }
        }

        public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.", nameof(orderId));

            try
            {
                var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order != null)
                {
                    order.OrderStatus = newStatus;
                    _db.Orders.Update(order);
                }
                else
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");
                }
            }
            catch (Exception ex) when (!(ex is KeyNotFoundException))
            {
                throw new ApplicationException($"An error occurred while updating status for order ID {orderId}.", ex);
            }
        }
    }
}
