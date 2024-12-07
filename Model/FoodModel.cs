using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.Model
{
    public class FoodModel : INotifyPropertyChanged
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        private int _quantityBuy;
        public int QuantityBuy
        {
            get => _quantityBuy;
            set
            {
                _quantityBuy = value;
                OnPropertyChanged(nameof(QuantityBuy));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
