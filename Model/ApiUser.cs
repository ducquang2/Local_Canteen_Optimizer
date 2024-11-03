using System.Text.Json.Serialization;

namespace Local_Canteen_Optimizer.Model
{
    public class ApiUser
    {
        [JsonPropertyName("user_id")]
        public int user_id { get; set; }
        [JsonPropertyName("username")]
        public string username { get; set; }
        [JsonPropertyName("full_name")]
        public string full_name { get; set; }
        [JsonPropertyName("phone_number")]
        public string phone_number { get; set; }
        [JsonPropertyName("role")]
        public string role { get; set; }
    }
}
