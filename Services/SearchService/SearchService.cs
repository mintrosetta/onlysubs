using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlySubs.Context;
using OnlySubs.Services.UserService;
using OnlySubs.ViewModels.Responses;

namespace OnlySubs.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly OnlySubsContext _db;
        private readonly IUserService _userService;

        public SearchService(OnlySubsContext db,
                             IUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<List<SearchResponse>> Finds(string search)
        {
            var users = _db.Users.Where(u => u.Username.Contains(search)).ToList();
            List<SearchResponse> result = new List<SearchResponse>();
            foreach(var user in users) 
            {
                SearchResponse searchResponse = new SearchResponse
                {
                    ImageName = user.ImageName,
                    Username = user.Username,
                    Follower = await _userService.FindFollowerCount(user.Id),
                    Following = await _userService.FindFollowingCount(user.Id)
                };
                result.Add(searchResponse);
            }
            result = result.OrderByDescending(u => u.Follower).ToList();
            return result;
        }
    }
}