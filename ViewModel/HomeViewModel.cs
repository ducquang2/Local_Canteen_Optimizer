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

        public HomeViewModel()
        {
            // Giả lập danh sách món ăn
            FoodItems = new ObservableCollection<FoodModel>
            {
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },
                new FoodModel { Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00 },

            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
