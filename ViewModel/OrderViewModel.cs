using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    class OrderViewModel : BaseViewModel
    {
        private readonly PageModel _pageModel;
        public DateOnly DisplayOrderDate
        {
            get { return _pageModel.OrderDate; }
            set { _pageModel.OrderDate = value; OnPropertyChanged(); }
        }

        public OrderViewModel()
        {
            _pageModel = new PageModel();
            DisplayOrderDate = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
