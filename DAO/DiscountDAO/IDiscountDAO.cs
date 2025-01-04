using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.DiscountDAO
{
    public interface IDiscountDAO
    {
        public Task<List<DiscountModel>> GetEligibleDiscount(double totalPrice);
        public Task<Tuple<int, List<DiscountModel>>> GetDiscountsAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending);
        public Task<DiscountModel> AddDiscountAsync(DiscountModel newDiscount);
        public Task<DiscountModel> UpdateDiscountAsync(DiscountModel newDiscount);
        public Task<bool> RemoveDiscountAsync(int discountId);
    }
}
