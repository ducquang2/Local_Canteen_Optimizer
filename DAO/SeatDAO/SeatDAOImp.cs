using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.SeatDAO
{
    /// <summary>
    /// Implementation of the Seat Data Access Object (DAO) interface.
    /// </summary>
    public class SeatDAOImp : ISeatDAO
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatDAOImp"/> class.
        /// </summary>
        public SeatDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }

        /// <summary>
        /// Asynchronously retrieves a list of seats.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="TableModel"/>.</returns>
        [ArmDot.Client.VirtualizeCode]
        public async Task<List<TableModel>> GetSeatsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetApiSeatsResponse>("api/v1/seats");
                var seats = response.results.Select(ConvertToTableModel).ToList();
                return new List<TableModel>(seats);
            }
            catch
            {
                // Handle errors if any
                return null;
            }
        }

        /// <summary>
        /// Converts an <see cref="ApiSeats"/> object to a <see cref="TableModel"/> object.
        /// </summary>
        /// <param name="seat">The <see cref="ApiSeats"/> object to convert.</param>
        /// <returns>The converted <see cref="TableModel"/> object.</returns>
        [ArmDot.Client.VirtualizeCode]
        private TableModel ConvertToTableModel(ApiSeats seat)
        {
            return new TableModel
            {
                tableId = seat.table_id,
                tableName = seat.table_name,
                isAvailable = seat.is_available,
                currentOrderId = seat.current_order_id,
                createAt = seat.created_at,
                updateAt = seat.updated_at
            };
        }

        /// <summary>
        /// Represents the response from the API for retrieving seats.
        /// </summary>
        public class GetApiSeatsResponse
        {
            /// <summary>
            /// Gets or sets the list of <see cref="ApiSeats"/> objects.
            /// </summary>
            [JsonPropertyName("results")]
            public List<ApiSeats> results { get; set; }
        }
    }
}
