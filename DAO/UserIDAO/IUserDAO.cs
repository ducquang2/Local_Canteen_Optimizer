using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.UserIDAO
{
    /// <summary>
    /// Interface for User Data Access Object.
    /// </summary>
    public interface IUserDAO
    {
        /// <summary>
        /// Asynchronously retrieves a paginated list of users.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="rowsPerPage">The number of rows per page.</param>
        /// <param name="keyword">The keyword to search for.</param>
        /// <param name="nameAscending">Sort order for names.</param>
        /// <returns>A tuple containing the total number of users and a list of UserModel objects.</returns>
        public Task<Tuple<int, List<UserModel>>> GetUsersAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending);

        /// <summary>
        /// Asynchronously retrieves a user by username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A UserModel object representing the user.</returns>
        public Task<UserModel> GetUserAsync(string username);

        /// <summary>
        /// Asynchronously adds a new user.
        /// </summary>
        /// <param name="newUser">The UserModel object representing the new user.</param>
        /// <returns>A UserModel object representing the added user.</returns>
        public Task<UserModel> AddUserAsync(UserModel newUser);

        /// <summary>
        /// Asynchronously updates an existing user.
        /// </summary>
        /// <param name="newUser">The UserModel object representing the user to update.</param>
        /// <returns>A UserModel object representing the updated user.</returns>
        public Task<UserModel> UpdateUserAsync(UserModel newUser);

        /// <summary>
        /// Asynchronously removes a user by user ID.
        /// </summary>
        /// <param name="userID">The ID of the user to remove.</param>
        /// <returns>A boolean indicating whether the removal was successful.</returns>
        public Task<bool> RemoveUserAsync(int userID);
    }
}
