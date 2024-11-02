using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    class HomeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FoodModel> FoodItems { get; set; }
        private CartViewModel _cartViewModel;

        public HomeViewModel()
        {

        }

        public HomeViewModel(CartViewModel cartViewModel)
        {
            _cartViewModel = cartViewModel;
            // Giả lập danh sách món ăn
            FoodItems = new ObservableCollection<FoodModel>
            {
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24000 },

            };
        }

        private void AddToCart(FoodModel foodItem)
        {
            _cartViewModel.AddItemToCart(new CartItemModel
            {
                Id = foodItem.ProductID,
                Name = foodItem.Name,
                Price = foodItem.Price
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
