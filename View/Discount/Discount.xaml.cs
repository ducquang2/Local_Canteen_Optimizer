using Local_Canteen_Optimizer.Model;
using Local_Canteen_Optimizer.View.Product;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Canteen_Optimizer.View.Discount
{
    public sealed partial class Discount : UserControl
    {
        private ListDiscount discountListControl;
        private AddDiscount addDiscountControl;
        private EditDiscount editDiscountControl;
        //private ProductViewModel productViewModel;
        public Discount()
        {
            this.InitializeComponent();
            //productViewModel = new ProductViewModel();
            //DataContext = productViewModel;

            // Khởi tạo danh sách sản phẩm
            discountListControl = new ListDiscount();
            discountListControl.AddDiscountRequested += OnAddDiscountRequested;
            discountListControl.EditDiscountRequested += OnEditDiscountRequested;

            // Khởi tạo form thêm sản phẩm
            addDiscountControl = new AddDiscount();
            addDiscountControl.SaveRequested += OnAddSaveRequested;
            addDiscountControl.CancelRequested += OnCancelRequested;

            editDiscountControl = new EditDiscount();
            editDiscountControl.SaveRequested += OnEditSaveRequested;
            editDiscountControl.CancelRequested += OnCancelRequested;

            // Hiển thị danh sách sản phẩm ban đầu
            DiscountsContent.Content = discountListControl;
        }

        private void OnAddDiscountRequested(object sender, EventArgs e)
        {
            // Khi nhấn nút Add Discount, chuyển sang form thêm sản phẩm
            DiscountsContent.Content = addDiscountControl;
        }
        private void OnEditDiscountRequested(object sender, DiscountModel discount)
        {
            editDiscountControl.SetDiscount(discount);
            DiscountsContent.Content = editDiscountControl;
        }

        private void OnAddSaveRequested(object sender, DiscountModel discount)
        {
            // Khi lưu sản phẩm, thêm vào danh sách và quay lại danh sách sản phẩm
            discountListControl.AddDiscount(discount);
            DiscountsContent.Content = discountListControl;
        }
        private void OnEditSaveRequested(object sender, DiscountModel discount)
        {
            discountListControl.UpdateDiscount(discount);
            DiscountsContent.Content = discountListControl; // Quay lại danh sách sản phẩm
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            // Khi hủy, quay lại danh sách sản phẩm
            DiscountsContent.Content = discountListControl;
        }
    }
}
