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
            var (totalItems, products) = await _dao.GetProductsAsync(null, null, null, true);
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
            _cartViewModel.AddItemToCart(new CartItemModel
            {
                Id = foodItem.ProductID,
                Name = foodItem.Name,
                Price = foodItem.Price
            });
        }

    }
}
