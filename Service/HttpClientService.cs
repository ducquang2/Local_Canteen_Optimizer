﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Service
{
    class HttpClientService
    {
        private static HttpClient _httpClient;

        static HttpClientService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://randomuser.me/api/")
            };
            //_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public static HttpClient GetHttpClient()
        {
            return _httpClient;
        }
    }
}
