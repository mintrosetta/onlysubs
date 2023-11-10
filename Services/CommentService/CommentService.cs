using System.Threading.Tasks;
using OnlySubs.Context;
using OnlySubs.Models.db;

namespace OnlySubs.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly OnlySubsContext _db;

        public CommentService(OnlySubsContext db)
        {
            _db = db;
        }

        public async Task CommentByPostId(string userId, string postId, string description)
        {
            PostsComment comment = new PostsComment
            {
                UserId = userId,
                PostId = postId,
                Description = description
            };
            _db.PostsComments.Add(comment);
            await _db.SaveChangesAsync();
        }
    }
}