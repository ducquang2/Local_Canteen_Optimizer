using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    class ManageUserViewModel : BaseViewModel
    {
        private readonly PageModel _pageModel;

        public ManageUserViewModel()
        {
            _pageModel = new PageModel();
        }
    }
}