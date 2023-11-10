using System.Threading.Tasks;
using OnlySubs.Models.db;

namespace OnlySubs.Services.BuyService
{
    public interface IBuyPostService
    {
        Task<Post> FindPost(string postId);
        Task<bool> IsSub(string postId, string userId);
        Task<bool> Buy(string userId, string postId);

    }
}