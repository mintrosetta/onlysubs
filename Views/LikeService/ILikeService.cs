using System.Threading.Tasks;

namespace OnlySubs.Views.LikeService
{
    public interface ILikeService
    {
        Task<bool> ValidatePost(string postId);
         Task<bool> IsLike(string postId, string userId);
         Task Like(string postId, string userId);
         Task Unlike(string postId, string userId);
    }
}