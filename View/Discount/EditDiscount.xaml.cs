using DocumentFormat.OpenXml.Vml;
using Local_Canteen_Optimizer.Model;
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
    public sealed partial class EditDiscount : UserControl
    {
        public event EventHandler<DiscountModel> SaveRequested;
        public event EventHandler CancelRequested;
        private DiscountModel currentDiscount;
        public EditDiscount()
        {
            this.InitializeComponent();
        }
        public void SetDiscount(DiscountModel discount)
        {
            currentDiscount = discount;
            NameTextBox.Text = discount.DiscountName;
            DescriptionTextBox.Text = discount.DiscountDescription;
            if(discount.DiscountType == "percentage")
            {
                TypeComboBox.SelectedIndex = 0;
            }
            else
            {
                TypeComboBox.SelectedIndex = 1;
            }
            ValueTextBox.Text = discount.DiscountValue.ToString();
            StartDatePicker.Date = discount.DiscountStartDate.Date;
            StartTimePicker.Time = discount.DiscountStartDate.TimeOfDay;
            EndDatePicker.Date = discount.DiscountEndDate.Date;
            EndTimePicker.Time = discount.DiscountEndDate.TimeOfDay;
            MinValueTextBox.Text = discount.DiscountMinOrderValue.ToString();
            MaxValueTextBox.Text = discount.DiscountMaxValue.ToString();


        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            // Reset error messages
            NameErrorText.Visibility = Visibility.Collapsed;
            DescriptionErrorText.Visibility = Visibility.Collapsed;
            ValueErrorText.Visibility = Visibility.Collapsed;
            MinValueErrorText.Visibility = Visibility.Collapsed;
            MaxValueErrorText.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                DescriptionErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (!double.TryParse(ValueTextBox.Text, out var value) || value < 0)
            {
                ValueErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (!double.TryParse(MinValueTextBox.Text, out var minValue) || minValue < 0)
            {
                MinValueErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (!double.TryParse(MaxValueTextBox.Text, out var maxValue) || maxValue < 0)
            {
                MaxValueErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // If there are errors, stop here
            if (hasError) return;
            var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString().ToLower();

            var selectedStartDate = StartDatePicker.Date;
            var selectedStartTime = StartTimePicker.Time;
            var selectedEndDate = EndDatePicker.Date;
            var selectedEndTime = EndTimePicker.Time;

            DateTime startDateTime = new DateTime(
                selectedStartDate.Year,
                selectedStartDate.Month,
                selectedStartDate.Day,
                selectedStartTime.Hours,
                selectedStartTime.Minutes,
                selectedStartTime.Seconds
            );
            DateTime endDateTime = new DateTime(
                selectedEndDate.Year,
                selectedEndDate.Month,
                selectedEndDate.Day,
                selectedEndTime.Hours,
                selectedEndTime.Minutes,
                selectedEndTime.Seconds
            );

            currentDiscount.DiscountName = NameTextBox.Text;
            currentDiscount.DiscountDescription = DescriptionTextBox.Text;
            currentDiscount.DiscountType = selectedType;
            currentDiscount.DiscountValue = double.Parse(ValueTextBox.Text);
            currentDiscount.DiscountStartDate = startDateTime;
            currentDiscount.DiscountEndDate = endDateTime;
            currentDiscount.DiscountMinOrderValue = double.Parse(MinValueTextBox.Text);
            currentDiscount.DiscountMaxValue = double.Parse(MaxValueTextBox.Text);

            SaveRequested?.Invoke(this, currentDiscount);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
