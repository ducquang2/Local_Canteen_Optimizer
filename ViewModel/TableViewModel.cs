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
    class TableViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TableModel> listTables { get; set; }
        public TableViewModel() {
            // Giả lập danh sách món ăn
            listTables = new ObservableCollection<TableModel>
            {
                new TableModel { Name = "Bàn 1", Status = "Trống" },
                new TableModel { Name = "Bàn 2", Status = "Đầy" },
                new TableModel { Name = "Bàn 3", Status = "Trống" },
                new TableModel { Name = "Bàn 4", Status = "Trống" },
                new TableModel { Name = "Bàn 5", Status = "Trống" },
                new TableModel { Name = "Bàn 6", Status = "Đầy" },
                new TableModel { Name = "Bàn 7", Status = "Trống" },
                new TableModel { Name = "Bàn 8", Status = "Trống" },
                new TableModel { Name = "Bàn 9", Status = "Trống" },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
