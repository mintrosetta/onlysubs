using System.Threading.Tasks;

namespace OnlySubs.Services.CommentService
{
    public interface ICommentService
    {
         Task CommentByPostId(string userId, string postId, string description);
    }
}