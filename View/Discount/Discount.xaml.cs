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
    /// <summary>
    /// Interaction logic for Discount UserControl.
    /// </summary>
    public sealed partial class Discount : UserControl
    {
        private ListDiscount discountListControl;
        private AddDiscount addDiscountControl;
        private EditDiscount editDiscountControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Discount"/> class.
        /// </summary>
        public Discount()
        {
            this.InitializeComponent();

            // Initialize discount list control
            discountListControl = new ListDiscount();
            discountListControl.AddDiscountRequested += OnAddDiscountRequested;
            discountListControl.EditDiscountRequested += OnEditDiscountRequested;

            // Initialize add discount control
            addDiscountControl = new AddDiscount();
            addDiscountControl.SaveRequested += OnAddSaveRequested;
            addDiscountControl.CancelRequested += OnCancelRequested;

            // Initialize edit discount control
            editDiscountControl = new EditDiscount();
            editDiscountControl.SaveRequested += OnEditSaveRequested;
            editDiscountControl.CancelRequested += OnCancelRequested;

            // Display initial discount list
            DiscountsContent.Content = discountListControl;
        }

        /// <summary>
        /// Handles the AddDiscountRequested event.
        /// </summary>
        private void OnAddDiscountRequested(object sender, EventArgs e)
        {
            DiscountsContent.Content = addDiscountControl;
        }

        /// <summary>
        /// Handles the EditDiscountRequested event.
        /// </summary>
        private void OnEditDiscountRequested(object sender, DiscountModel discount)
        {
            editDiscountControl.SetDiscount(discount);
            DiscountsContent.Content = editDiscountControl;
        }

        /// <summary>
        /// Handles the SaveRequested event for adding a discount.
        /// </summary>
        private void OnAddSaveRequested(object sender, DiscountModel discount)
        {
            discountListControl.AddDiscount(discount);
            DiscountsContent.Content = discountListControl;
        }

        /// <summary>
        /// Handles the SaveRequested event for editing a discount.
        /// </summary>
        private void OnEditSaveRequested(object sender, DiscountModel discount)
        {
            discountListControl.UpdateDiscount(discount);
            DiscountsContent.Content = discountListControl;
        }

        /// <summary>
        /// Handles the CancelRequested event.
        /// </summary>
        private void OnCancelRequested(object sender, EventArgs e)
        {
            DiscountsContent.Content = discountListControl;
        }
    }
}
