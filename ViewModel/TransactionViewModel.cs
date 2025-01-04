using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    /// <summary>
    /// ViewModel class for managing transactions.
    /// </summary>
    class TransactionViewModel : BaseViewModel
    {
        private readonly PageModel _pageModel;

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        public decimal TransactionAmount
        {
            get { return _pageModel.TransactionValue; }
            set { _pageModel.TransactionValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionViewModel"/> class.
        /// </summary>
        public TransactionViewModel()
        {
            _pageModel = new PageModel();
            TransactionAmount = 5638;
        }
    }
}
