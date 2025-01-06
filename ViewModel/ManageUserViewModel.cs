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
    /// <summary>
    /// ViewModel class for managing user page.
    /// </summary>
    class ManageUserViewModel : BaseViewModel
    {
        private IUserDAO _dao = null;

        /// <summary>
        /// Gets or sets the keyword for searching users.
        /// </summary>
        public string Keyword { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether the user names should be sorted in ascending order.
        /// </summary>
        public bool NameAcending { get; set; } = true;

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of rows per page.
        /// </summary>
        public int RowsPerPage { get; set; } = 10;

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        public int TotalItems { get; set; } = 0;

        /// <summary>
        /// Gets or sets the collection of user items.
        /// </summary>
        public ObservableCollection<UserModel> UserItems { get; set; }

        /// <summary>
        /// Initializes the ViewModel and loads the users.
        /// </summary>
        public async Task Init()
        {
            _dao = new UserDAOImp();
            UserItems = new ObservableCollection<UserModel>();
            await LoadUsersAsync();
        }

        /// <summary>
        /// Loads the users for the specified page.
        /// </summary>
        /// <param name="page">The page number to load.</param>
        public async Task Load(int page)
        {
            CurrentPage = page;
            await LoadUsersAsync();
        }

        /// <summary>
        /// Loads the users asynchronously.
        /// </summary>
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

        /// <summary>
        /// Gets a user asynchronously by username.
        /// </summary>
        /// <param name="username">The username of the user to get.</param>
        /// <returns>The user model.</returns>
        public async Task<UserModel> GetUserAsync(string username)
        {
            return await _dao.GetUserAsync(username);
        }

        /// <summary>
        /// Adds a new user asynchronously.
        /// </summary>
        /// <param name="user">The user model to add.</param>
        public async Task AddUser(UserModel user)
        {
            UserModel newUser = await _dao.AddUserAsync(user);
            if (newUser != null)
            {
                UserItems.Add(newUser);
            }
        }

        /// <summary>
        /// Edits an existing user asynchronously.
        /// </summary>
        /// <param name="user">The user model to edit.</param>
        public async Task EditUser(UserModel user)
        {
            UserModel editedUser = await _dao.UpdateUserAsync(user);
            if (editedUser != null)
            {
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