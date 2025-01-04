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
        public Task<Tuple<int, List<OrderModel>>> GetAllOrders(int? page, int? rowsPerPage, bool dateAscending);
        public Task<List<FoodModel>> GetAllOrderItems(int orderId);
        public Task<bool> CheckOut(int tableId, int orderId, string note);
        public Task<bool> UpdateOrderItems(OrderModel orderModel);

        public Task<bool> UpdateOrder(OrderModel orderModel);
    }
}
