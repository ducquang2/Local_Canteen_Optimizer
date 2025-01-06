using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.OrderDAO
{
    /// <summary>
    /// Interface for Order Data Access Object.
    /// </summary>
    public interface IOrderDAO
    {
        /// <summary>
        /// Adds a new order asynchronously.
        /// </summary>
        /// <param name="orderModel">The order model to add.</param>
        /// <returns>The added order model.</returns>
        public Task<OrderModel> AddOrderAsync(OrderModel orderModel);

        /// <summary>
        /// Gets the order model from a specific table.
        /// </summary>
        /// <param name="tableId">The table ID.</param>
        /// <returns>The order model from the table.</returns>
        public Task<OrderModel> GetOrderModelFromTable(int tableId);

        /// <summary>
        /// Updates the table after an order.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="tableId">The table ID.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public Task<bool> UpdateTableAfterOrder(int orderId, int tableId);

        /// <summary>
        /// Gets all orders with pagination and sorting options.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="dateAscending">Sort by date in ascending order if true, otherwise descending.</param>
        /// <returns>A tuple containing the total number of orders and a list of order models.</returns>
        public Task<Tuple<int, List<OrderModel>>> GetAllOrders(int? page, int? rowsPerPage, bool dateAscending);

        /// <summary>
        /// Gets all items of a specific order.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <returns>A list of food models in the order.</returns>
        public Task<List<FoodModel>> GetAllOrderItems(int orderId);


        /// <summary>
        /// Checks out an order for a specific table.
        /// </summary>
        /// <param name="tableId">The table ID.</param>
        /// <param name="orderId">The order ID.</param>
        /// <returns>True if the checkout was successful, otherwise false.</returns>
        public Task<bool> CheckOut(int tableId, int orderId);

        /// <summary>
        /// Updates the items of an order.
        /// </summary>
        /// <param name="orderModel">The order model with updated items.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public Task<bool> UpdateOrderItems(OrderModel orderModel);
      
        /// <summary>
        /// Updates the order.
        /// </summary>
        /// <param name="orderModel">The order model with updated value.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public Task<bool> UpdateOrder(OrderModel orderModel);

        /// <summary>
        /// Applies a discount to an order.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="promotionId">The promotion ID.</param>
        /// <returns>The discounted total price, or null if the discount could not be applied.</returns>
        public Task<double?> ApplyDiscount(int orderId, int promotionId);

        /// <summary>
        /// Applies reward points to an order.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="totalPrice">The total price of the order.</param>
        /// <param name="phoneNumber">The customer's phone number.</param>
        /// <param name="rewardPoints">The number of reward points to apply.</param>
        /// <returns>True if the reward points were successfully applied, otherwise false.</returns>
        public Task<bool> ApplyRewardPoint(int orderId, double totalPrice, string phoneNumber, int rewardPoints);

        /// <summary>
        /// Adds reward points to a customer's account based on the total price of an order.
        /// </summary>
        /// <param name="totalPrice">The total price of the order.</param>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>True if the reward points were successfully added, otherwise false.</returns>
        public Task<bool> AddRewardPoints(double totalPrice, int customerId);
    }
}
