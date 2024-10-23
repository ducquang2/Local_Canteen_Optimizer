using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    class TransactionViewModel : BaseViewModel
    {
        private readonly PageModel _pageModel;
        public decimal TransactionAmount
        {
            get { return _pageModel.TransactionValue; }
            set { _pageModel.TransactionValue = value; OnPropertyChanged(); }
        }

        public TransactionViewModel()
        {
            _pageModel = new PageModel();
            TransactionAmount = 5638;
        }
    }
}
