using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Local_Canteen_Optimizer.ViewModel
{
    class CartViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CartItemModel> CartItems { get; set; }
        public ICommand RemoveItemCommand { get; set; }

        public double Subtotal => CartItems.Sum(item => item.Price);
        public double Tax => Subtotal * 0.1; // Giả sử thuế 10%
        public double Total => Subtotal + Tax;

        public CartViewModel()
        {
            // Giả lập dữ liệu giỏ hàng
            CartItems = new ObservableCollection<CartItemModel>
            {
                new CartItemModel { Name = "Schezwan Egg Noodles", Price = 25.00 },
                new CartItemModel { Name = "Spicy Shrimp Soup", Price = 40.00 }
            };

            RemoveItemCommand = new RelayCommand<CartItemModel>(RemoveItem);

        }

        private void RemoveItem(CartItemModel item)
        {
            CartItems.Remove(item);
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
