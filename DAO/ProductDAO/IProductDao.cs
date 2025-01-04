using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.ProductDAO
{
    public interface IProductDao
    {
        public Task<Tuple<int,List<FoodModel>>> GetProductsAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending, double? minPrice, double? maxPrice);
        public Task<FoodModel> AddProductAsync(FoodModel newProduct);
        public Task<FoodModel> UpdateProductAsync(FoodModel newProduct);
        public Task<bool> RemoveProductAsync(int productID);
    }
}
