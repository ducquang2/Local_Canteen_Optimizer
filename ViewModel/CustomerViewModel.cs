using DocumentFormat.OpenXml.Spreadsheet;
using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.DAO.CustomerDAO;
using Local_Canteen_Optimizer.DAO.DiscountDAO;
using Local_Canteen_Optimizer.DAO.ProductDAO;
using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.View.Customer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Local_Canteen_Optimizer.ViewModel
{
    class CustomerViewModel : BaseViewModel
    {
        private readonly PageModel _pageModel;
        public int CustomerID
        {
            get { return _pageModel.CustomerCount; }
            set { _pageModel.CustomerCount = value; OnPropertyChanged(); }
        }

        public CustomerViewModel()
        {
            _pageModel = new PageModel();
            CustomerID = 100528;
        }
    }
}
