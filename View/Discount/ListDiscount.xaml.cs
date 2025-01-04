using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
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

namespace Local_Canteen_Optimizer.View.Discount
{
    public sealed partial class ListDiscount : UserControl
    {
        /// <summary>
        /// ViewModel for managing discounts.
        /// </summary>
        DiscountViewModel discountViewModel;

        /// <summary>
        /// Event triggered when a request to add a discount is made.
        /// </summary>
        public event EventHandler AddDiscountRequested;

        /// <summary>
        /// Event triggered when a request to edit a discount is made.
        /// </summary>
        public event EventHandler<DiscountModel> EditDiscountRequested;

        /// <summary>
        /// Initializes a new instance of the ListDiscount class.
        /// </summary>
        public ListDiscount()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        /// <summary>
        /// Asynchronously initializes the ViewModel and updates paging information.
        /// </summary>
        public async Task InitializeAsync()
        {
            discountViewModel = new DiscountViewModel();
            await discountViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

        /// <summary>
        /// Updates the paging information for the discount list.
        /// </summary>
        void UpdatePagingInfo_bootstrap()
        {
            var infoList = new List<object>();
            for (int i = 1; i <= discountViewModel.TotalPages; i++)
            {
                infoList.Add(new
                {
                    Page = i,
                    Total = discountViewModel.TotalPages
                });
            };

            pagesComboBox.ItemsSource = infoList;
            pagesComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the click event for adding a discount.
        /// </summary>
        private void AddDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            AddDiscountRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the click event for editing a discount.
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var discount = (sender as Button).Tag as DiscountModel;
            EditDiscountRequested?.Invoke(this, discount);
        }

        /// <summary>
        /// Adds a new discount.
        /// </summary>
        public async void AddDiscount(DiscountModel discount)
        {
            try
            {
                await discountViewModel.AddDiscount(discount);
                await MessageHelper.ShowSuccessMessage("Add new discount successful", App.m_window.Content.XamlRoot);
            }
            catch (Exception e)
            {
                await MessageHelper.ShowErrorMessage("Fail to add new discount", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        public async void UpdateDiscount(DiscountModel discount)
        {
            await discountViewModel.UpdateDiscount(discount);
        }

        /// <summary>
        /// Handles the click event for removing a discount.
        /// </summary>
        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is DiscountModel discount)
            {
                await discountViewModel.DeleteDiscount(discount);
            }
        }

        /// <summary>
        /// Handles the selection changed event for the pages combo box.
        /// </summary>
        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                discountViewModel.Load(item.Page);
            }
        }

        /// <summary>
        /// Handles the selection changed event for the sort order combo box.
        /// </summary>
        public void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    bool isAscending = bool.Parse(selectedItem.Tag.ToString());
                    discountViewModel.LoadDiscountSort(isAscending);
                }
            }
        }

        /// <summary>
        /// Handles the text changed event for the keyword text box.
        /// </summary>
        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the click event for the search button.
        /// </summary>
        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            await handleSearchButtonClick();
        }

        /// <summary>
        /// Handles the search button click event asynchronously.
        /// </summary>
        public async Task handleSearchButtonClick()
        {
            await discountViewModel.Load(1);
            UpdatePagingInfo_bootstrap();
        }
    }
}
