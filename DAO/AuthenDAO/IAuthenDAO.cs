using Local_Canteen_Optimizer.Model;
using System.Threading.Tasks;

namespace Local_Canteen_Optimizer.DAO.AuthenDAO
{
    public interface IAuthenDAO
    {
        public Task<AuthenModel> LoginAsync(string username, string password);
        public bool LogoutAsync();
    }
}
