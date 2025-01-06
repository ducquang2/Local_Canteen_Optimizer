using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.DiscountDAO
{
    /// <summary>
    /// Interface for Discount Data Access Object.
    /// </summary>
    public interface IDiscountDAO
    {
        /// <summary>
        /// Gets the eligible discounts based on the total price.
        /// </summary>
        /// <param name="totalPrice">The total price to evaluate discounts for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of eligible discounts.</returns>
        public Task<List<DiscountModel>> GetEligibleDiscount(double totalPrice);

        /// <summary>
        /// Gets a paginated list of discounts based on the provided parameters.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="keyword">The keyword to filter discounts.</param>
        /// <param name="nameAscending">A boolean indicating if the sorting should be in ascending order by name.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the total count and a list of discounts.</returns>
        public Task<Tuple<int, List<DiscountModel>>> GetDiscountsAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending);

        /// <summary>
        /// Adds a new discount.
        /// </summary>
        /// <param name="newDiscount">The new discount to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added discount.</returns>
        public Task<DiscountModel> AddDiscountAsync(DiscountModel newDiscount);

        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        /// <param name="newDiscount">The discount with updated information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated discount.</returns>
        public Task<DiscountModel> UpdateDiscountAsync(DiscountModel newDiscount);

        /// <summary>
        /// Removes a discount by its identifier.
        /// </summary>
        /// <param name="discountId">The identifier of the discount to remove.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the removal was successful.</returns>
        public Task<bool> RemoveDiscountAsync(int discountId);
    }
}
