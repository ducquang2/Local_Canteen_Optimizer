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
        DiscountViewModel discountViewModel;
        public event EventHandler AddDiscountRequested;
        public event EventHandler<DiscountModel> EditDiscountRequested;
        public ListDiscount()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            discountViewModel = new DiscountViewModel();
            await discountViewModel.Init();
            UpdatePagingInfo_bootstrap();
        }

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

        private void AddDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            AddDiscountRequested?.Invoke(this, EventArgs.Empty);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy sản phẩm từ nút Edit
            var discount = (sender as Button).Tag as DiscountModel;
            EditDiscountRequested?.Invoke(this, discount);
        }
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
        public async void UpdateDiscount(DiscountModel discount)
        {
            await discountViewModel.UpdateDiscount(discount);
        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is DiscountModel discount)
            {
                await discountViewModel.DeleteDiscount(discount);
            }
        }

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = pagesComboBox.SelectedItem;

            if (item != null)
            {
                discountViewModel.Load(item.Page);
            }
        }

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


        private void keywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            await handleSearchButtonClick();
        }

        public async Task handleSearchButtonClick()
        {
            await discountViewModel.Load(1);
            UpdatePagingInfo_bootstrap();
        }

        
    }
}
