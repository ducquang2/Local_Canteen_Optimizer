using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class ApiProduct
    {
        [JsonPropertyName("product_id")]
        public int product_id { get; set; }
        [JsonPropertyName("product_name")]
        public string product_name { get; set; }
        [JsonPropertyName("price")]
        public double price { get; set; }
        [JsonPropertyName("description")]
        public string description { get; set; }
        [JsonPropertyName("stock_quantity")]
        public int stock_quantity { get; set; }
        [JsonPropertyName("image_url")]
        public string image_url { get; set; }
    }
}
