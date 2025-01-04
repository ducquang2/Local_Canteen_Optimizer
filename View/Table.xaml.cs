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

namespace Local_Canteen_Optimizer.View
{
    public sealed partial class Table : UserControl
    {
        public TableViewModel tableViewModel;
        public event EventHandler CancelRequested;
        public event EventHandler<int> SaveTableRequested;
        public Table()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            tableViewModel = new TableViewModel();
            await tableViewModel.Init();
        }

        public void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
        private void ChooseTable_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int tableId = (int)button.Tag;
                SaveTableRequested.Invoke(this, tableId);
            }
        }
    }
}
