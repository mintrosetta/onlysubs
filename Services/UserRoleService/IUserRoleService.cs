using System.Threading.Tasks;

namespace OnlySubs.Services.UserRoleService
{
    public interface IUserRoleService
    {
        Task AddRoleByUserIdAsync(string userId, int roleId);
        Task<string> GetRoleByUserId(string userId);
    }
}