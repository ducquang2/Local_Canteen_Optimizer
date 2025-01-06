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
    /// <summary>
    /// ViewModel class for managing report data and charts.
    /// </summary>
    public class ReportViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Gets or sets the sales series data.
        /// </summary>
        public ISeries[] SalesSeries { get; set; }

        /// <summary>
        /// Gets or sets the X axes for the sales chart.
        /// </summary>
        public Axis[] SalesXAxes { get; set; }

        /// <summary>
        /// Gets or sets the Y axes for the sales chart.
        /// </summary>
        public Axis[] SalesYAxes { get; set; }

        /// <summary>
        /// Gets or sets the user growth series data.
        /// </summary>
        public ISeries[] UserGrowthSeries { get; set; }

        /// <summary>
        /// Gets or sets the X axes for the user growth chart.
        /// </summary>
        public Axis[] UserGrowthXAxes { get; set; }

        /// <summary>
        /// Gets or sets the Y axes for the user growth chart.
        /// </summary>
        public Axis[] UserGrowthYAxes { get; set; }

        /// <summary>
        /// Gets or sets the most product series data.
        /// </summary>
        public ISeries[] MostProductSeries { get; set; }

        /// <summary>
        /// Gets or sets the X axes for the most product chart.
        /// </summary>
        public Axis[] TotalQuantityXAxes { get; set; }

        /// <summary>
        /// Gets or sets the Y axes for the most product chart.
        /// </summary>
        public Axis[] MostProductYAxes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportViewModel"/> class.
        /// </summary>
        public ReportViewModel()
        {
            _httpClient = HttpClientService.GetHttpClient();
            LoadSalesData();
            LoadUserGrowthData();
            LoadMostProductData();
        }

        /// <summary>
        /// Loads the sales data asynchronously.
        /// </summary>
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

        /// <summary>
        /// Loads the user growth data asynchronously.
        /// </summary>
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

        /// <summary>
        /// Loads the most product data asynchronously.
        /// </summary>
        private async void LoadMostProductData()
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("userToken"))
                {
                    string userToken = localSettings.Values["userToken"] as string;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

                    var mostProductData = await _httpClient.GetFromJsonAsync<List<MostProductData>>("api/v1/chart/most-product");
                    MostProductSeries = new ISeries[]
                    {
                            new ColumnSeries<double>
                            {
                                Values = mostProductData.ConvertAll(data => double.Parse(data.TotalQuantity)),
                                Name = "Total Quantity",
                                Fill = new SolidColorPaint(SKColors.Orange)
                            }
                    };

                    TotalQuantityXAxes = new Axis[]
                    {
                            new Axis
                            {
                                Labels = mostProductData.ConvertAll(data => data.ProductName).ToArray()
                            }
                    };

                    MostProductYAxes = new Axis[]
                    {
                            new Axis
                            {
                                Labeler = value => value.ToString()
                            }
                    };

                    OnPropertyChanged(nameof(MostProductSeries));
                    OnPropertyChanged(nameof(TotalQuantityXAxes));
                    OnPropertyChanged(nameof(MostProductYAxes));
                }
                else
                {
                    throw new UnauthorizedAccessException("User not authenticated");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching most product data: {ex.Message}");
            }
        }

        /// <summary>
        /// Represents the sales data.
        /// </summary>
        public class SalesData
        {
            /// <summary>
            /// Gets or sets the month of the sales data.
            /// </summary>
            [JsonPropertyName("month")]
            public DateTime Month { get; set; }

            /// <summary>
            /// Gets or sets the total sales.
            /// </summary>
            [JsonPropertyName("total_sales")]
            public string TotalSales { get; set; }
        }

        /// <summary>
        /// Represents the user growth data.
        /// </summary>
        public class UserGrowthData
        {
            /// <summary>
            /// Gets or sets the month of the user growth data.
            /// </summary>
            [JsonPropertyName("month")]
            public DateTime Month { get; set; }

            /// <summary>
            /// Gets or sets the user count.
            /// </summary>
            [JsonPropertyName("user_count")]
            public string UserCount { get; set; }
        }

        /// <summary>
        /// Represents the most product data.
        /// </summary>
        public class MostProductData
        {
            /// <summary>
            /// Gets or sets the month of the most product data.
            /// </summary>
            [JsonPropertyName("month")]
            public DateTime Month { get; set; }

            /// <summary>
            /// Gets or sets the product name.
            /// </summary>
            [JsonPropertyName("product_name")]
            public string ProductName { get; set; }

            /// <summary>
            /// Gets or sets the total quantity.
            /// </summary>
            [JsonPropertyName("total_quantity")]
            public string TotalQuantity { get; set; }
        }
    }
}