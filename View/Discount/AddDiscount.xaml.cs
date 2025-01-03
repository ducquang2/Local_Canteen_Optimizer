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
    public sealed partial class AddDiscount : UserControl
    {
        public event EventHandler<DiscountModel> SaveRequested;
        public event EventHandler CancelRequested;
        public AddDiscount()
        {
            this.InitializeComponent();
            StartDatePicker.Date = DateTimeOffset.Now;
            StartTimePicker.Time = new TimeSpan(7, 0, 0);
            EndDatePicker.Date = DateTimeOffset.Now.AddDays(1);
            EndTimePicker.Time = new TimeSpan(7, 0, 0);
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

            var discount = new DiscountModel
            {
                DiscountName = NameTextBox.Text,
                DiscountDescription = DescriptionTextBox.Text,
                DiscountType = selectedType,
                DiscountValue = double.Parse(ValueTextBox.Text),
                DiscountStartDate = startDateTime,
                DiscountEndDate = endDateTime,
                DiscountMinOrderValue = double.Parse(MinValueTextBox.Text),
                DiscountMaxValue = double.Parse(MaxValueTextBox.Text)
            };

            SaveRequested?.Invoke(this, discount);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
