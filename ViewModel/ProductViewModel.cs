using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using Local_Canteen_Optimizer.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Local_Canteen_Optimizer.ViewModel
{
    class ProductViewModel : BaseViewModel
    {
        //private readonly UserService _apiService;
        //public ObservableCollection<ApiUser> Users { get; private set; }

        //public ProductViewModel()
        //{
        //    _apiService = new UserService();
        //    Users = new ObservableCollection<ApiUser>();
        //    LoadUsersAsync();
        //}

        //private async Task LoadUsersAsync()
        //{
        //    var users = await _apiService.GetUsersAsync();
        //    Users.Clear();
        //    foreach (var user in users)
        //    {
        //        Users.Add(user);
        //    }
        //    OnPropertyChanged(nameof(Users));
        //}

        public ObservableCollection<FoodModel> FoodItems { get; set; }

        public ProductViewModel()
        {
            // Giả lập danh sách món ăn
            FoodItems = new ObservableCollection<FoodModel>
            {
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
                new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},

            };
        }

        public void AddFoodItem(FoodModel food)
        {
            FoodItems.Add(food);
        }

        public void UpdateProduct(FoodModel product)
        {
            // Tìm và cập nhật sản phẩm trong danh sách
            var existingProductIndex = FoodItems.IndexOf(FoodItems.FirstOrDefault(p => p.Name == product.Name));
            if (existingProductIndex >= 0)
            {
                FoodItems[existingProductIndex] = new FoodModel
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    ImageSource = product.ImageSource,
                    Price = product.Price,
                    Quantity = product.Quantity
                };
            }
        }

        public void DeleteProduct(FoodModel product)
        {
            if (FoodItems.Contains(product))
            {
                FoodItems.Remove(product);
            }
        }
    }
}
