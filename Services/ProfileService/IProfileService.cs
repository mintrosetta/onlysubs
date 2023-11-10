using System.Collections.Generic;
using System.Threading.Tasks;
using OnlySubs.Models;
using OnlySubs.ViewModels.Responses;

namespace OnlySubs.Services.ProfileService
{
    public interface IProfileService
    {
         Task<bool> IsFollow(string userId, string followUserId);
         Task<UserProfileResponse> FindDetail(string userId, string profileUserId);
        //  Task<List<ProfilePostImage>> FindPosts(string userId, string profileUserId);
    }
}