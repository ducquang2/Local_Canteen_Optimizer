using Local_Canteen_Optimizer.DAO.ProductDAO;
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
        private IProductDao _dao = null;
        public string Keyword { get; set; } = "";
        public bool NameAcending { get; set; } = true;
        public int CurrentPage { get; set; } = 0;
        public int RowsPerPage { get; set; } = 10;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public ObservableCollection<FoodModel> FoodItems { get; set; }

        public async Task Init()
        {
            _dao = new ProductDAOImp();
            FoodItems = new ObservableCollection<FoodModel>();
            await LoadProductsAsync();
        }
        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadProductsAsync();
        }

        public async Task LoadProductsAsync()
        {
            var (totalItems,products) = await _dao.GetProductsAsync(CurrentPage, RowsPerPage, Keyword, NameAcending);
            FoodItems.Clear();
            foreach (var item in products)
            {
                FoodItems.Add(item);
            }
            OnPropertyChanged(nameof(FoodItems));

            TotalItems = totalItems;
            TotalPages = (TotalItems / RowsPerPage) + ((TotalItems % RowsPerPage == 0) ? 0 : 1);

        }

        //public void UpdatePageOptions()
        //{
        //    PageOptions.Clear();
        //    for (int i = 1; i <= TotalPages; i++)
        //    {
        //        PageOptions.Add($"{i}/{TotalPages}");
        //    }
        //}

        public async Task AddFoodItem(FoodModel food)
        {
            FoodModel newFood = await _dao.AddProductAsync(food);
            if (newFood != null)
            {
                FoodItems.Add(newFood);
            }
        }
        public async Task UpdateProduct(FoodModel product)
        {
            FoodModel updateFood = await _dao.UpdateProductAsync(product);
            if(updateFood != null)
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
        }
        public async Task DeleteProduct(FoodModel product)
        {
            bool isRemoved = await _dao.RemoveProductAsync(int.Parse(product.ProductID));
            if (isRemoved)
            {
                if (FoodItems.Contains(product))
                {
                    FoodItems.Remove(product);
                }
            }
        }

        //public ProductViewModel()
        //{
        //    // Giả lập danh sách món ăn
        //    FoodItems = new ObservableCollection<FoodModel>
        //    {
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},
        //        new FoodModel { ProductID = "1", Name = "Schezwan Egg Noodles", ImageSource = "ms-appx:///Assets/ImgFood/mi-quang.png", Price = 24.00, Quantity = 20},

        //    };
        //}
    }
}
