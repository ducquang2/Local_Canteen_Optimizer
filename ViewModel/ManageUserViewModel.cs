using Local_Canteen_Optimizer.DAO.UserIDAO;
using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.ViewModel
{
    class ManageUserViewModel : BaseViewModel
    {
        private IUserDAO _dao = null;
        public string Keyword { get; set; } = "";
        public bool NameAcending { get; set; } = true;
        public int CurrentPage { get; set; } = 0;
        public int RowsPerPage { get; set; } = 10;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;

        public ObservableCollection<UserModel> UserItems { get; set; }

        public async Task Init()
        {
            _dao = new UserDAOImp();
            UserItems = new ObservableCollection<UserModel>();
            await LoadUsersAsync();
        }

        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadUsersAsync();
        }

        public async Task LoadUsersAsync()
        {
            var (totalItems, users) = await _dao.GetUsersAsync(CurrentPage, RowsPerPage, Keyword, NameAcending);
            UserItems.Clear();
            foreach (var item in users)
            {
                UserItems.Add(item);
            }
            OnPropertyChanged(nameof(UserItems));

            TotalItems = totalItems;
            TotalPages = (TotalItems / RowsPerPage) + ((TotalItems % RowsPerPage == 0) ? 0 : 1);
        }

        public async Task<UserModel> GetUserAsync(string username)
        {
            return await _dao.GetUserAsync(username);
        }

        public async Task AddUser(UserModel user)
        {
            UserModel newUser = await _dao.AddUserAsync(user);
            if (newUser != null)
            {
                UserItems.Add(newUser);
            }
        }

        public async Task EditUser(UserModel user)
        {
            UserModel editedUser = await _dao.UpdateUserAsync(user);
            if (editedUser != null)
            {
                // Tìm và cập nhật sản phẩm trong danh sách
                var existingUserIndex = UserItems.IndexOf(UserItems.FirstOrDefault(p => p.UserID == editedUser.UserID));
                if (existingUserIndex >= 0)
                {
                    UserItems[existingUserIndex] = new UserModel
                    {
                        UserID = editedUser.UserID,
                        Username = editedUser.Username,
                        Full_name = editedUser.Full_name,
                        Phone_number = editedUser.Phone_number,
                        Role = editedUser.Role
                    };
                }
            }
        }
    }
}