using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    class ProductViewModel : BaseViewModel
    {
        private readonly PageModel _pageModel;
        public string ProductAvailability
        {
            get { return _pageModel.ProductStatus; }
            set { _pageModel.ProductStatus = value; OnPropertyChanged(); }
        }

        public ProductViewModel()
        {
            _pageModel = new PageModel();
            ProductAvailability = "Out of Stock";
        }
    }
}
