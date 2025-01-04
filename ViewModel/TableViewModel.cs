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
    public class TableViewModel : BaseViewModel
    {
        private ISeatDAO _dao = null;
        public ObservableCollection<TableModel> listTables { get; set; }
        public TableViewModel() {
            
        }

        public async Task Init()
        {
            _dao = new SeatDAOImp();
            listTables = new ObservableCollection<TableModel>();
            await LoadProductsAsync();
        }

        public void updateTable(TableModel newTable) {
            if(newTable == null) return;
            var table = listTables.FirstOrDefault(t => t.tableId == newTable.tableId);
            if (table != null)
            {
                var index = listTables.IndexOf(table);
                newTable.tableName = table.tableName;
                listTables[index] = newTable;
            }
        }

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
