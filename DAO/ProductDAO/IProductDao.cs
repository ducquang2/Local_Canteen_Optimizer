using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.ProductDAO
{
    /// <summary>
    /// Interface for product data access object.
    /// </summary>
    public interface IProductDao
    {
        /// <summary>
        /// Retrieves a list of products asynchronously.
        /// </summary>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="rowsPerPage">The number of rows per page for pagination.</param>
        /// <param name="keyword">The keyword to search for products.</param>
        /// <param name="nameAscending">Sort order for product names.</param>
        /// <param name="minPrice">The minimum price filter.</param>
        /// <param name="maxPrice">The maximum price filter.</param>
        /// <returns>A tuple containing the total count and a list of products.</returns>
        public Task<Tuple<int, List<FoodModel>>> GetProductsAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending, double? minPrice, double? maxPrice);

        /// <summary>
        /// Adds a new product asynchronously.
        /// </summary>
        /// <param name="newProduct">The new product to add.</param>
        /// <returns>The added product.</returns>
        public Task<FoodModel> AddProductAsync(FoodModel newProduct);

        /// <summary>
        /// Updates an existing product asynchronously.
        /// </summary>
        /// <param name="newProduct">The product with updated information.</param>
        /// <returns>The updated product.</returns>
        public Task<FoodModel> UpdateProductAsync(FoodModel newProduct);

        /// <summary>
        /// Removes a product asynchronously.
        /// </summary>
        /// <param name="productID">The ID of the product to remove.</param>
        /// <returns>A boolean indicating whether the removal was successful.</returns>
        public Task<bool> RemoveProductAsync(int productID);
    }
}
