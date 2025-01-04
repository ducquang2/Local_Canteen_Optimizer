using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.Service;
using Local_Canteen_Optimizer.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Order
{
    public sealed partial class ListOrders : UserControl
    {
        /// <summary>
        /// ViewModel for managing orders.
        /// </summary>
        public OrderViewModel orderViewModel;

        /// <summary>
        /// Event triggered when order detail view is requested.
        /// </summary>
        public event EventHandler<OrderModel> ViewOrderDetailRequested;

        /// <summary>
        /// Initializes a new instance of the ListOrders class.
        /// </summary>
        public ListOrders()
        {
            this.InitializeComponent();
            orderViewModel = new OrderViewModel();
            InitializeAsync();
        }

        /// <summary>
        /// Asynchronously initializes the ViewModel and updates paging information.
        /// </summary>
        public async Task InitializeAsync()
        {
            orderViewModel = new OrderViewModel();
            await orderViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

        /// <summary>
        /// Updates the paging information for the orders.
        /// </summary>
        void UpdatePagingInfo_bootstrap()
        {
            var infoList = new List<object>();
            for (int i = 1; i <= orderViewModel.TotalPages; i++)
            {
                infoList.Add(new
                {
                    Page = i,
                    Total = orderViewModel.TotalPages
                });
            };

            pagesComboBox.ItemsSource = infoList;
            pagesComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the selection change event of the pages combo box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                OrderDataServices.Instance.Load(item.Page);
            }
        }

        /// <summary>
        /// Handles the selection change event of the sort order combo box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    bool isAscending = bool.Parse(selectedItem.Tag.ToString());
                    OrderDataServices.Instance.LoadOrderSort(isAscending);
                }
            }
        }

        /// <summary>
        /// Handles the click event of the view button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            var order = (sender as Button).Tag as OrderModel;
            ViewOrderDetailRequested?.Invoke(this, order);
        }
    }
}
