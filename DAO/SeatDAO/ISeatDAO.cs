using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.SeatDAO
{
    /// <summary>
    /// Interface for Seat Data Access Object.
    /// </summary>
    public interface ISeatDAO
    {
        /// <summary>
        /// Asynchronously retrieves a list of seats.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of TableModel objects.</returns>
        public Task<List<TableModel>> GetSeatsAsync();
    }
}
