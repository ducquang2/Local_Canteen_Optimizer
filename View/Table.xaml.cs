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
    /// <summary>
    /// Interaction logic for Table.xaml
    /// </summary>
    public sealed partial class Table : UserControl
    {
        /// <summary>
        /// ViewModel for the Table view.
        /// </summary>
        public TableViewModel tableViewModel;

        /// <summary>
        /// Event triggered when the cancel action is requested.
        /// </summary>
        public event EventHandler CancelRequested;

        /// <summary>
        /// Event triggered when the save table action is requested.
        /// </summary>
        public event EventHandler<int> SaveTableRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        public Table()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        /// <summary>
        /// Asynchronously initializes the Table view model.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InitializeAsync()
        {
            tableViewModel = new TableViewModel();
            await tableViewModel.Init();
        }

        /// <summary>
        /// Handles the click event of the BackToHome button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the click event of the ChooseTable button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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
