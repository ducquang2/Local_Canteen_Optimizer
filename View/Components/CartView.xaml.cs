using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View
{
    public sealed partial class CartView : UserControl
    {
        public event EventHandler AddTableRequested;
        public event EventHandler<TableModel> HoldCartRequested;
        public event EventHandler<TableModel> CheckOutRequested;

        public CartView()
        {
            this.InitializeComponent();
            this.DataContext = new CartViewModel();
        }

        private void RemoveCartItemButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FoodModel itemToRemove = button?.Tag as FoodModel;

            if (itemToRemove != null)
            {
                var cartViewModel = this.DataContext as CartViewModel;
                if (cartViewModel != null)
                {
                    // Gọi hàm xóa item trong CartViewModel
                    cartViewModel.RemoveItem(itemToRemove);
                }
            }
        }

        private async void holdCartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartViewModel = this.DataContext as CartViewModel;
            if (cartViewModel != null)
            {
                TableModel table = await cartViewModel.HoldCart();
                HoldCartRequested?.Invoke(this, table);
            }
        }

        private void SelectTable_Click(object sender, RoutedEventArgs e)
        {
            AddTableRequested?.Invoke(this, EventArgs.Empty);
        }

        private async void checkoutCartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartViewModel = this.DataContext as CartViewModel;
            if (cartViewModel != null)
            {
                TableModel table = await cartViewModel.CheckOut();
                CheckOutRequested?.Invoke(this, table);
            }
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            NotePopup.IsOpen = true;
        }

        private async void SaveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as CartViewModel;
            if (viewModel != null)
            {
                viewModel.Note = NoteTextBox.Text;
                bool success = await viewModel.UpdateOrderNoteAsync();
                if (success)
                {
                    // Optionally show a success message
                    NotePopup.IsOpen = false;
                }
                else
                {
                    // Optionally show an error message
                }
            }
        }
    }
}