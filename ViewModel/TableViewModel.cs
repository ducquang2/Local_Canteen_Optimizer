using Local_Canteen_Optimizer.DAO.ProductDAO;
using Local_Canteen_Optimizer.DAO.SeatDAO;
using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    /// <summary>
    /// ViewModel for managing table data.
    /// </summary>
    public class TableViewModel : BaseViewModel
    {
        private ISeatDAO _dao = null;

        /// <summary>
        /// Collection of table models.
        /// </summary>
        public ObservableCollection<TableModel> listTables { get; set; }

        /// <summary>
        /// Initializes a new instance of the TableViewModel class.
        /// </summary>
        public TableViewModel()
        {

        }

        /// <summary>
        /// Initializes the ViewModel by setting up the DAO and loading products.
        /// </summary>
        public async Task Init()
        {
            _dao = new SeatDAOImp();
            listTables = new ObservableCollection<TableModel>();
            await LoadProductsAsync();
        }

        /// <summary>
        /// Updates the table with new data.
        /// </summary>
        /// <param name="newTable">The new table data to update.</param>
        public void updateTable(TableModel newTable)
        {
            if (newTable == null) return;
            var table = listTables.FirstOrDefault(t => t.tableId == newTable.tableId);
            if (table != null)
            {
                var index = listTables.IndexOf(table);
                newTable.tableName = table.tableName;
                listTables[index] = newTable;
            }
        }

        /// <summary>
        /// Loads the products asynchronously.
        /// </summary>
        public async Task LoadProductsAsync()
        {
            var seats = await _dao.GetSeatsAsync();
            if (seats == null)
            {
                await MessageHelper.ShowErrorMessage("Fetch data is fail", App.m_window.Content.XamlRoot);
                return;
            }
            listTables.Clear();
            foreach (var item in seats)
            {
                listTables.Add(item);
            }
            OnPropertyChanged(nameof(TableModel));
        }
    }
}
