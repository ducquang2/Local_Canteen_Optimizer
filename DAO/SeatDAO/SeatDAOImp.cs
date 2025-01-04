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
    public class SeatDAOImp : ISeatDAO
    {
        private readonly HttpClient _httpClient;

        public SeatDAOImp()
        {
            _httpClient = HttpClientService.GetHttpClient();
        }
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
                // Xử lý lỗi nếu có
                return null;
            }
        }

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

        public class GetApiSeatsResponse
        {
            [JsonPropertyName("results")]
            public List<ApiSeats> results { get; set; }
        }
    }
}
