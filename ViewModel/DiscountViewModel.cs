using Local_Canteen_Optimizer.Commands;
using Local_Canteen_Optimizer.DAO.DiscountDAO;
using Local_Canteen_Optimizer.Helper;
using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Local_Canteen_Optimizer.ViewModel
{

    /// <summary>
    /// ViewModel class for managing discounts.
    /// </summary>
    public class DiscountViewModel : BaseViewModel
    {
        private IDiscountDAO _dao = null;

        /// <summary>
        /// Keyword for searching discounts.
        /// </summary>
        public string Keyword { get; set; } = "";

        /// <summary>
        /// Flag indicating if the start date is ascending.
        /// </summary>
        public bool StartDateAcending { get; set; } = true;

        /// <summary>
        /// Current page number.
        /// </summary>
        public int CurrentPage { get; set; } = 0;

        /// <summary>
        /// Number of rows per page.
        /// </summary>
        public int RowsPerPage { get; set; } = 10;

        /// <summary>
        /// Total number of pages.
        /// </summary>
        public int TotalPages { get; set; } = 0;

        /// <summary>
        /// Total number of items.
        /// </summary>
        public int TotalItems { get; set; } = 0;

        /// <summary>
        /// Collection of discounts.
        /// </summary>
        public ObservableCollection<DiscountModel> discounts { get; set; }

        /// <summary>
        /// Command for deleting a food item.
        /// </summary>
        public ICommand DeleteFoodCommand { get; set; }

        /// <summary>
        /// Constructor for DiscountViewModel.
        /// </summary>
        public DiscountViewModel()
        {
            _dao = new DiscountDAOImp();
        }

        /// <summary>
        /// Initializes the ViewModel.
        /// </summary>
        public async Task Init()
        {
            try
            {
                discounts = new ObservableCollection<DiscountModel>();
                DeleteFoodCommand = new RelayCommand<DiscountModel>(async (food) => await ConfirmAndDeleteFoodItem(food));
                await LoadDiscountAsync();
            }
            catch
            {
                await MessageHelper.ShowErrorMessage("Can't get any discounts", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Loads discounts for a specific page.
        /// </summary>
        /// <param name="page">The page number to load.</param>
        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadDiscountAsync();
        }

        /// <summary>
        /// Loads discounts with sorting.
        /// </summary>
        /// <param name="startDateAcending">Flag indicating if the start date is ascending.</param>
        public async Task LoadDiscountSort(bool startDateAcending)
        {
            StartDateAcending = startDateAcending;
            await LoadDiscountAsync();
        }

        /// <summary>
        /// Loads discounts asynchronously.
        /// </summary>
        public async Task LoadDiscountAsync()
        {
            var (totalItems, results) = await _dao.GetDiscountsAsync(CurrentPage, RowsPerPage, Keyword, StartDateAcending);
            discounts.Clear();
            foreach (var item in results)
            {
                discounts.Add(item);
            }
            OnPropertyChanged(nameof(discounts));

            TotalItems = totalItems;
            TotalPages = (TotalItems / RowsPerPage) + ((TotalItems % RowsPerPage == 0) ? 0 : 1);
        }

        /// <summary>
        /// Gets eligible discounts based on total price.
        /// </summary>
        /// <param name="totalPrice">The total price to check for eligibility.</param>
        /// <returns>A list of eligible discounts.</returns>
        public async Task<List<DiscountModel>> getEligibleDiscount(double totalPrice)
        {
            try
            {
                List<DiscountModel> discounts = await _dao.GetEligibleDiscount(totalPrice);
                if (discounts != null)
                {
                    return discounts;
                }
                else
                {
                    await MessageHelper.ShowErrorMessage("fail to get discounts", App.m_window.Content.XamlRoot);
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Confirms and deletes a food item.
        /// </summary>
        /// <param name="discount">The discount to delete.</param>
        private async Task ConfirmAndDeleteFoodItem(DiscountModel discount)
        {
            if (discount == null)
            {
                // Display error message if needed
                return;
            }

            // Display confirmation dialog
            bool isConfirmed = await MessageHelper.ShowConfirmationDialog(
                $"Are you sure you want to delete: {discount.DiscountName}?",
                "Confirm customer deletion",
                App.m_window.Content.XamlRoot
            );

            if (isConfirmed)
            {
                await DeleteDiscount(discount);
            }
        }

        /// <summary>
        /// Adds a new discount.
        /// </summary>
        /// <param name="discount">The discount to add.</param>
        public async Task AddDiscount(DiscountModel discount)
        {
            DiscountModel newDiscount = await _dao.AddDiscountAsync(discount);
            if (newDiscount != null)
            {
                discounts.Add(newDiscount);
            }
            else
            {
                throw new Exception("Fail to add new discount");
            }
        }

        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        /// <param name="discount">The discount to update.</param>
        public async Task UpdateDiscount(DiscountModel discount)
        {
            DiscountModel updateDiscount = await _dao.UpdateDiscountAsync(discount);
            if (updateDiscount != null)
            {
                // Find and update the discount in the list
                var existingDiscountIndex = discounts.IndexOf(discounts.FirstOrDefault(p => p.DiscountID == discount.DiscountID));
                if (existingDiscountIndex >= 0)
                {
                    discounts[existingDiscountIndex] = new DiscountModel
                    {
                        DiscountID = discount.DiscountID,
                        DiscountName = discount.DiscountName,
                        DiscountDescription = discount.DiscountDescription,
                        DiscountType = discount.DiscountType,
                        DiscountValue = discount.DiscountValue,
                        DiscountMinOrderValue = discount.DiscountMinOrderValue,
                        DiscountMaxValue = discount.DiscountMaxValue,
                        DiscountStartDate = discount.DiscountStartDate,
                        DiscountEndDate = discount.DiscountEndDate
                    };
                    await MessageHelper.ShowSuccessMessage("Update discount successful", App.m_window.Content.XamlRoot);
                }
                else
                {
                    await MessageHelper.ShowErrorMessage("Fail to update discount", App.m_window.Content.XamlRoot);
                }
            }
            else
            {
                await MessageHelper.ShowErrorMessage("Fail to update discount", App.m_window.Content.XamlRoot);
            }
        }

        /// <summary>
        /// Deletes a discount.
        /// </summary>
        /// <param name="discount">The discount to delete.</param>
        public async Task DeleteDiscount(DiscountModel discount)
        {
            bool isRemoved = await _dao.RemoveDiscountAsync(int.Parse(discount.DiscountID));
            if (isRemoved)
            {
                if (discounts.Contains(discount))
                {
                    discounts.Remove(discount);
                    await MessageHelper.ShowSuccessMessage("Remove discount successful", App.m_window.Content.XamlRoot);
                }
            }
            else
            {
                await MessageHelper.ShowErrorMessage("Fail to remove discount", App.m_window.Content.XamlRoot);
            }
        }
    }
}
