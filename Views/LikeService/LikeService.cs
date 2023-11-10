using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlySubs.Context;
using OnlySubs.Models.db;

namespace OnlySubs.Views.LikeService
{
    public class LikeService : ILikeService
    {
        private readonly OnlySubsContext _db;

        public LikeService(OnlySubsContext db)
        {
            _db = db;
        }
        public async Task<bool> ValidatePost(string postId)
        {
            return await _db.Posts.FirstOrDefaultAsync(post => post.Id == postId) != null;
        }

        public async Task<bool> IsLike(string postId, string userId)
        {
            return await _db.PostsLikes.Where(like => like.PostId == postId)
                                       .Where(like => like.UserId == userId)
                                       .FirstOrDefaultAsync()
                                        != null;
        }

        public async Task Like(string postId, string userId)
        {
            PostsLike like = new PostsLike
            {
                UserId = userId,
                PostId = postId
            };
            _db.PostsLikes.Add(like);
            await _db.SaveChangesAsync();
        }

        public async Task Unlike(string postId, string userId)
        {
            PostsLike like = await _db.PostsLikes.Where(like => like.PostId == postId)
                                       .Where(like => like.UserId == userId)
                                       .FirstOrDefaultAsync();
            _db.PostsLikes.Remove(like);
            await _db.SaveChangesAsync();
        }
    }
}