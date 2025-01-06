using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.DAO.AuthenDAO;
using Local_Canteen_Optimizer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Local_Canteen_Optimizer.ViewModel
{
    /// <summary>
    /// ViewModel class for managing navigation between different views.
    /// </summary>
    public class NavigationViewModel : BaseViewModel
    {
        /// <summary>
        /// Event triggered when navigation is requested.
        /// </summary>
        public event Action<Type> NavigationRequested;

        private object _currentView;
        /// <summary>
        /// Gets or sets the current view.
        /// </summary>
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Command for navigating to the Customers view.
        /// </summary>
        public ICommand CustomersCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Home view.
        /// </summary>
        public ICommand HomeCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Orders view.
        /// </summary>
        public ICommand OrdersCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Products view.
        /// </summary>
        public ICommand ProductsCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Reports view.
        /// </summary>
        public ICommand ReportsCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Settings view.
        /// </summary>
        public ICommand SettingsCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Tables view.
        /// </summary>
        public ICommand TablesCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Transactions view.
        /// </summary>
        public ICommand TransactionsCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Manage User view.
        /// </summary>
        public ICommand ManageUserCommand { get; set; }
        /// <summary>
        /// Command for navigating to the Discount view.
        /// </summary>
        public ICommand DiscountCommand { get; set; }

        /// <summary>
        /// Navigates to the Customers view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Customer(object obj) => CurrentView = new CustomerViewModel();
        /// <summary>
        /// Navigates to the Home view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Home(object obj) => CurrentView = new HomeViewModel();
        /// <summary>
        /// Navigates to the Orders view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Order(object obj) => CurrentView = new OrderViewModel();
        /// <summary>
        /// Navigates to the Products view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Product(object obj) => CurrentView = new ProductViewModel();
        /// <summary>
        /// Navigates to the Reports view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Report(object obj) => CurrentView = new ReportViewModel();
        /// <summary>
        /// Navigates to the Settings view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Setting(object obj) => CurrentView = new SettingViewModel();
        /// <summary>
        /// Navigates to the Tables view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Table(object obj) => CurrentView = new TableViewModel();
        /// <summary>
        /// Navigates to the Transactions view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Transaction(object obj) => CurrentView = new TransactionViewModel();
        /// <summary>
        /// Navigates to the Manage User view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void ManageUser(object obj) => CurrentView = new ManageUserViewModel();
        /// <summary>
        /// Navigates to the Discount view.
        /// </summary>
        /// <param name="obj">The parameter passed to the command.</param>
        private void Discount(object obj) => CurrentView = new DiscountViewModel();

        /// <summary>
        /// Initializes a new instance of the NavigationViewModel class.
        /// </summary>
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
            ManageUserCommand = new RelayCommand<Object>(ManageUser);
            DiscountCommand = new RelayCommand<Object>(Discount);

            // Startup Page
            CurrentView = new HomeViewModel();
        }

        /// <summary>
        /// Requests navigation to the specified page type.
        /// </summary>
        /// <param name="pageType">The type of the page to navigate to.</param>
        private void NavigateTo(Type pageType)
        {
            NavigationRequested?.Invoke(pageType);
        }
    }
}
