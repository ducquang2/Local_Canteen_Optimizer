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
    class OrderViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<OrderModel> Orders => OrderDataServices.Instance.Orders;

        public OrderViewModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
