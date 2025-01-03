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
    public class DiscountViewModel : BaseViewModel
    {
        private IDiscountDAO _dao = null;
        public string Keyword { get; set; } = "";
        public bool StartDateAcending { get; set; } = true;
        public int CurrentPage { get; set; } = 0;
        public int RowsPerPage { get; set; } = 10;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public ObservableCollection<DiscountModel> discounts { get; set; }
        public ICommand DeleteFoodCommand { get; set; }
        public DiscountViewModel()
        {
            _dao = new DiscountDAOImp();
        }
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

        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadDiscountAsync();
        }

        public async Task LoadDiscountSort(bool startDateAcending)
        {
            StartDateAcending = startDateAcending;
            await LoadDiscountAsync();
        }

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

        private async Task ConfirmAndDeleteFoodItem(DiscountModel discount)
        {
            if (discount == null)
            {
                // Hiển thị thông báo lỗi nếu cần
                return;
            }

            // Hiển thị hộp thoại xác nhận
            bool isConfirmed = await MessageHelper.ShowConfirmationDialog(
                $"Bạn có chắc chắn muốn xoá sản phẩm: {discount.DiscountName}?",
                "Xác nhận thêm sản phẩm",
                App.m_window.Content.XamlRoot
            );

            if (isConfirmed)
            {
                await DeleteDiscount(discount);
            }
        }

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

        public async Task UpdateDiscount(DiscountModel discount)
        {
            DiscountModel updateDiscount = await _dao.UpdateDiscountAsync(discount);
            if (updateDiscount != null)
            {
                // Tìm và cập nhật sản phẩm trong danh sách
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
