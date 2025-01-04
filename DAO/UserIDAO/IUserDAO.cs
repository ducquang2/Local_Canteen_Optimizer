using Local_Canteen_Optimizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.UserIDAO
{
    public interface IUserDAO
    {
        public Task<Tuple<int, List<UserModel>>> GetUsersAsync(int? page, int? rowsPerPage, string keyword, bool nameAscending);

        public Task<UserModel> GetUserAsync(string username);
        public Task<UserModel> AddUserAsync(UserModel newUser);
        public Task<UserModel> UpdateUserAsync(UserModel newUser);
        public Task<bool> RemoveUserAsync(int userID);
    }
}
