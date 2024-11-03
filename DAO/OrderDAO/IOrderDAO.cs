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
        public Task<bool> AddOrderAsync(OrderModel orderModel);
    }
}
