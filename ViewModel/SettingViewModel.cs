using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    /// <summary>
    /// ViewModel class for managing settings.
    /// </summary>
    class SettingViewModel : BaseViewModel
    {
        private readonly PageModel _pageModel;

        /// <summary>
        /// Gets or sets the settings status.
        /// </summary>
        public bool Settings
        {
            get { return _pageModel.LocationStatus; }
            set { _pageModel.LocationStatus = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel"/> class.
        /// </summary>
        public SettingViewModel()
        {
            _pageModel = new PageModel();
            Settings = true;
        }
    }
}
