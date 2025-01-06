using Local_Canteen_Optimizer.Model;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.AuthenDAO
{
    /// <summary>
    /// Defines methods for user authentication.
    /// </summary>
    public interface IAuthenDAO
    {
        /// <summary>
        /// Asynchronously logs in a user with the specified username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the authentication model.</returns>
        public Task<AuthenModel> LoginAsync(string username, string password);

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>True if the user was successfully logged out; otherwise, false.</returns>
        public bool LogoutAsync();
    }
}
