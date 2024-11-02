using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
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
    public class CartViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CartItemModel> CartItems { get; set; }
        public ICommand RemoveItemCommand { get; set; }

        public double Subtotal => CartItems.Sum(item => item.Price);
        public double Tax => Subtotal * 0;
        public double Total => Subtotal + Tax;

        public CartViewModel()
        {
            // Giả lập dữ liệu giỏ hàng
            CartItems = new ObservableCollection<CartItemModel>
            {
                //new CartItemModel {Id="1", Name = "Schezwan Egg Noodles", Price = 25000 },
                //new CartItemModel {Id="2", Name = "Spicy Shrimp Soup", Price = 40000 }
            };

            RemoveItemCommand = new RelayCommand<CartItemModel>(RemoveItem);

        }

        public void AddItemToCart(CartItemModel item)
        {
            CartItems.Add(item);
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        public void RemoveItem(CartItemModel item)
        {
            CartItems.Remove(item);
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        public void HoldCart()
        {
           
            var order = new OrderModel
            {
                TableNumber = 1,
                OrderTime = DateTime.Now,
                OrderDetails = CartItems.ToList(),
                Total = Total
            };

            OrderDataServices.Instance.Orders.Add(order);
            CartItems.Clear();
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
