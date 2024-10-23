using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Local_Canteen_Optimizer.ViewModel
{
    public class NavigationViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand CustomersCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand OrdersCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand ReportsCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand TablesCommand { get; set; }
        public ICommand TransactionsCommand { get; set; }

        private void Customer(object obj) => CurrentView = new CustomerViewModel();
        private void Home(object obj) => CurrentView = new HomeViewModel();
        private void Order(object obj) => CurrentView = new OrderViewModel();
        private void Product(object obj) => CurrentView = new ProductViewModel();
        private void Report(object obj) => CurrentView = new ReportViewModel();
        private void Setting(object obj) => CurrentView = new SettingViewModel();
        private void Table(object obj) => CurrentView = new TableViewModel();
        private void Transaction(object obj) => CurrentView = new TransactionViewModel();

        public NavigationViewModel()
        {
            CustomersCommand = new RelayCommand<Object>(Customer);
            HomeCommand = new RelayCommand<Object>(Home);
            OrdersCommand = new RelayCommand<Object>(Order);
            ProductsCommand = new RelayCommand<Object>(Product);
            ReportsCommand = new RelayCommand<Object>(Report);
            SettingsCommand = new RelayCommand<Object>(Setting);
            TablesCommand = new RelayCommand<Object>(Table);
            TransactionsCommand = new RelayCommand<Object>(Transaction);

            // Startup Page
            CurrentView = new HomeViewModel();
        }
    }
}
