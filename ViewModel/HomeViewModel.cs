using Local_Canteen_Optimizer.DAO.ProductDAO;
using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    /// <summary>
    /// ViewModel class for managing home page.
    /// </summary>
    class HomeViewModel : BaseViewModel
    {
        private IProductDao _dao = null;

        /// <summary>
        /// Keyword for searching products.
        /// </summary>
        public string keyword { get; set; } = "";

        private double _minPrice;
        private double _maxPrice;

        /// <summary>
        /// Minimum price filter for products.
        /// </summary>
        public double minPrice
        {
            get => _minPrice;
            set
            {
                if (double.TryParse(value.ToString(), out double result))
                {
                    _minPrice = result;
                }
                else
                {
                    _minPrice = -1;
                }
                OnPropertyChanged(nameof(minPrice));
            }
        }

        /// <summary>
        /// Maximum price filter for products.
        /// </summary>
        public double maxPrice
        {
            get => _maxPrice;
            set
            {
                if (double.TryParse(value.ToString(), out double result))
                {
                    _maxPrice = result;
                }
                else
                {
                    _maxPrice = -1;
                }
                OnPropertyChanged(nameof(maxPrice));
            }
        }

        /// <summary>
        /// Collection of food items to be displayed.
        /// </summary>
        public ObservableCollection<FoodModel> FoodItems { get; set; }

        private CartViewModel _cartViewModel;

        /// <summary>
        /// Initializes a new instance of the HomeViewModel class with a specified CartViewModel.
        /// </summary>
        /// <param name="cartViewModel">The CartViewModel instance.</param>
        public HomeViewModel(CartViewModel cartViewModel)
        {
            _cartViewModel = cartViewModel;
            _dao = new ProductDAOImp();
            FoodItems = new ObservableCollection<FoodModel>();
            LoadProductsAsync();
        }

        /// <summary>
        /// Loads products asynchronously from the data source.
        /// </summary>
        private async Task LoadProductsAsync()
        {
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, null, true, _minPrice, _maxPrice);
            if (products == null)
            {
                await MessageHelper.ShowErrorMessage("Can't get any products", App.m_window.Content.XamlRoot);
                return;
            }
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));
        }

        /// <summary>
        /// Searches for products asynchronously based on the keyword.
        /// </summary>
        public async Task searchProductsAsync()
        {
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, keyword, true, _minPrice, _maxPrice);
            if (totalItems == 0)
            {
                await MessageHelper.ShowErrorMessage("Can't get any products", App.m_window.Content.XamlRoot);
                return;
            }
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));
        }

        /// <summary>
        /// Filters products asynchronously based on the price range.
        /// </summary>
        public async Task filterProductsAsync()
        {
            if (_minPrice < 0 || _maxPrice < 0)
            {
                return;
            }
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, keyword, true, _minPrice, _maxPrice);
            if (totalItems == 0)
            {
                await MessageHelper.ShowErrorMessage("Can't get any products", App.m_window.Content.XamlRoot);
                return;
            }
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));
        }

        /// <summary>
        /// Default constructor for HomeViewModel.
        /// </summary>
        public HomeViewModel()
        {
        }

        /// <summary>
        /// Adds a food item to the cart.
        /// </summary>
        /// <param name="foodItem">The food item to add to the cart.</param>
        private void AddToCart(FoodModel foodItem)
        {
            _cartViewModel.AddItemToCart(foodItem);
        }
    }
}
