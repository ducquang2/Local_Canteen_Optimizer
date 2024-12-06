using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Local_Canteen_Optimizer.Service;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Local_Canteen_Optimizer.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;

        public ISeries[] SalesSeries { get; set; }
        public Axis[] SalesXAxes { get; set; }
        public Axis[] SalesYAxes { get; set; }

        public ISeries[] UserGrowthSeries { get; set; }
        public Axis[] UserGrowthXAxes { get; set; }
        public Axis[] UserGrowthYAxes { get; set; }

        public ReportViewModel()
        {
            _httpClient = HttpClientService.GetHttpClient();
            LoadSalesData();
            LoadUserGrowthData();
        }

        private async void LoadSalesData()
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var salesData = await _httpClient.GetFromJsonAsync<List<SalesData>>("api/v1/chart/sales");
                    SalesSeries = new ISeries[]
                    {
                        new ColumnSeries<double>
                        {
                            Values = salesData.ConvertAll(data => double.Parse(data.TotalSales)),
                            Name = "Sales",
                            Fill = new SolidColorPaint(SKColors.Blue)
                        },
                    };

                    SalesXAxes = new Axis[]
                    {
                        new Axis
                        {
                            Labels = salesData.ConvertAll(data => data.Month.ToString("MMM yyyy")).ToArray()
                        },
                    };

                    SalesYAxes = new Axis[]
                    {
                        new Axis
                        {
                            Labeler = value => value.ToString("C")
                        }
                    };

                    OnPropertyChanged(nameof(SalesSeries));
                    OnPropertyChanged(nameof(SalesXAxes));
                    OnPropertyChanged(nameof(SalesYAxes));
                } 
                else
                {
                    throw new UnauthorizedAccessException("User not authenticated");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sales data: {ex.Message}");
            }
        }

        private async void LoadUserGrowthData()
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var userGrowthData = await _httpClient.GetFromJsonAsync<List<UserGrowthData>>("api/v1/chart/user-growth");
                    UserGrowthSeries = new ISeries[]
                    {
                        new LineSeries<double>
                        {
                            Values = userGrowthData.ConvertAll(data => double.Parse(data.UserCount)),
                            Name = "User Growth",
                            Stroke = new SolidColorPaint(SKColors.Green),
                            Fill = null
                        }
                    };

                    UserGrowthXAxes = new Axis[]
                    {
                        new Axis
                        {
                            Labels = userGrowthData.ConvertAll(data => data.Month.ToString("MMM yyyy")).ToArray()
                        }
                    };

                    UserGrowthYAxes = new Axis[]
                    {
                        new Axis
                        {
                            Labeler = value => value.ToString()
                        }
                    };

                    OnPropertyChanged(nameof(UserGrowthSeries));
                    OnPropertyChanged(nameof(UserGrowthXAxes));
                    OnPropertyChanged(nameof(UserGrowthYAxes));
                }
                else
                {
                    throw new UnauthorizedAccessException("User not authenticated");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user growth data: {ex.Message}");
            }
        }

        public class SalesData
        {
            [JsonPropertyName("month")]
            public DateTime Month { get; set; }

            [JsonPropertyName("total_sales")]
            public string TotalSales { get; set; }
        }

        public class UserGrowthData
        {
            [JsonPropertyName("month")]
            public DateTime Month { get; set; }

            [JsonPropertyName("user_count")]
            public string UserCount { get; set; }
        }
    }
}