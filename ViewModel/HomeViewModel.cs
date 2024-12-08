using Local_Canteen_Optimizer.DAO.ProductDAO;
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
    class HomeViewModel : BaseViewModel
    {
        private IProductDao _dao = null;
        public string keyword { get; set; } = "";
        private double _minPrice;
        private double _maxPrice;
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
                    _maxPrice = -1; // Hoặc giá trị mặc định khác
                }
                OnPropertyChanged(nameof(maxPrice));
            }
        }
        public ObservableCollection<FoodModel> FoodItems { get; set; }
        private CartViewModel _cartViewModel;

        public HomeViewModel(CartViewModel cartViewModel)
        {
            _cartViewModel = cartViewModel;
            _dao = new ProductDAOImp();
            FoodItems = new ObservableCollection<FoodModel>();
            LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, null, true, _minPrice, _maxPrice);
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));
        }

        public async Task searchProductsAsync()
        {
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, keyword ,true, _minPrice, _maxPrice);
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));
        }

        public async Task filterProductsAsync()
        {
            if(_minPrice < 0 || _maxPrice < 0)
            {
                return;
            }   
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, keyword, true, _minPrice, _maxPrice);
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));
        }

        public HomeViewModel()
        {
            
        }

        //public HomeViewModel(CartViewModel cartViewModel)
        //{
        //    _cartViewModel = cartViewModel;
        //    // Giả lập danh sách món ăn
        //    FoodItems = new ObservableCollection<FoodModel>
        //    {
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
        //        new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },

        //    };
        //}

        private void AddToCart(FoodModel foodItem)
        {
            _cartViewModel.AddItemToCart(foodItem);
        }

    }
}
