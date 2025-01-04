using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.SeatDAO
{
    public interface ISeatDAO
    {
        public Task<List<TableModel>> GetSeatsAsync();
    }
}
