using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Service
{
    class HttpClientService
    {
        /// <summary>
        /// The static HttpClient instance.
        /// </summary>
        private static HttpClient _httpClient;

        /// <summary>
        /// Static constructor to initialize the HttpClient with a base address.
        /// </summary>
        static HttpClientService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://8080-idx-local-canteen-pos-1732536380411.cluster-a3grjzek65cxex762e4mwrzl46.cloudworkstations.dev/")
            };
            //_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Gets the static HttpClient instance.
        /// </summary>
        /// <returns>The HttpClient instance.</returns>
        public static HttpClient GetHttpClient()
        {
            return _httpClient;
        }
    }
}
