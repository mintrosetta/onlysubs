using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlySubs.Context;
using OnlySubs.Models.db;
using OnlySubs.Services.UserResourceService;

namespace OnlySubs.Services.BuyService
{
    public class BuyPostService : IBuyPostService
    {
        private readonly OnlySubsContext _db;
        private readonly IUserResourceService _userResourceService;

        public BuyPostService(OnlySubsContext db,
                              IUserResourceService userResourceService)
        {
            _db = db;
            _userResourceService = userResourceService;
        }

        public Task<Post> FindPost(string postId)
        {
            return _db.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<bool> IsSub(string postId, string userId)
        {
            return await _db.UsersPostsSubs.Where(p => p.PostId == postId)
                                           .Where(p => p.UserId == userId)
                                           .FirstOrDefaultAsync() 
                                           != null;
        }
        public async Task<bool> Buy(string userId, string postId)
        {
            PostsPrice price = await _db.PostsPrices.FirstOrDefaultAsync(p => p.PostId == postId);
            int? money = await _userResourceService.FindMoney(userId);

            if(price.Price > money) return false;

            UsersPostsSub postsSub = new UsersPostsSub
            {
                UserId = userId,
                PostId = postId
            };
            _db.UsersPostsSubs.Add(postsSub);
            await _db.SaveChangesAsync();

            await _userResourceService.SubtrackMoney(price.Price, userId);

            Post post = await FindPost(price.PostId);
            await _userResourceService.AddMoney(price.Price, post.UserId);

            await _userResourceService.AddKrama(5, userId);
            await _userResourceService.AddKrama(5, post.UserId);
            return true;
        }
    }
}