using System.Threading.Tasks;
using OnlySubs.Models.db;
using OnlySubs.ViewModels.Requests;

namespace OnlySubs.Services.UserService
{
    public interface IUserService
    {
        Task CreateAsync(UserRegisterRequest userRegisterRequest);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByUserIdAsync(string userId);
        Task UpdateAsync(UserUpdateRequest userUpdateRequest, string userId);
        Task RemoveAsync(string userId);
        Task<int> FindFollowerCount(string userId);
        Task<int> FindFollowingCount(string userId);   
        Task FollowToggle(string userId, string FollowUserId);
    }
}