using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.OrderDAO
{
    public interface IOrderDAO
    {
        public Task<OrderModel> AddOrderAsync(OrderModel orderModel);
        public Task<OrderModel> GetOrderModelFromTable(int tableId);
        public Task<bool> UpdateTableAfterOrder(int orderId, int tableId);
        public Task<List<OrderModel>> GetAllOrders();
        public Task<List<CartItemModel>> GetAllOrderItems(int orderId);
        public Task<bool> CheckOut(int tableId);
    }
}
