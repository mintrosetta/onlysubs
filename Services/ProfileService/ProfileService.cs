using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlySubs.Context;
using OnlySubs.Models;
using OnlySubs.Models.db;
using OnlySubs.Services.UserResourceService;
using OnlySubs.Services.UserService;
using OnlySubs.ViewModels.Responses;

namespace OnlySubs.Services.ProfileService
{
    public class ProfileService : IProfileService
    {
        private readonly OnlySubsContext _db;
        private readonly IUserResourceService _userResource;
        private readonly IUserService _userService;

        public ProfileService(OnlySubsContext db,
                              IUserResourceService userResource,
                              IUserService userService)
        {
            _db = db;
            _userResource = userResource;
            _userService = userService;
        }

        public async Task<UserProfileResponse> FindDetail(string userId, string profileUserId)
        {
            User user = await _db.Users.Where(user => user.Id == profileUserId).FirstOrDefaultAsync();
            var resource = await _userResource.FindResource(user.Id);

            UserProfileResponse profileResponse = new UserProfileResponse
            {
                ImageName = user.ImageName,
                Username = user.Username,
                Krama = resource.Krama,
                Follower = await _userService.FindFollowerCount(profileUserId),
                Following = await _userService.FindFollowingCount(profileUserId),
                Description = user.Description,
                PostsImage = await FindPosts(userId, profileUserId)
            };

            return profileResponse;
        }

        private async Task<List<ProfilePostImage>> FindPosts(string userId, string profileUserId)
        {
            var posts = _db.Posts.Where(post => post.UserId == profileUserId).ToList();

            List<ProfilePostImage> imagePosts = new List<ProfilePostImage>();

            foreach (var post in posts)
            {
                string image = await _db.PostsImages.Where(i => i.PostId == post.Id).Select(i => i.ImageName).FirstOrDefaultAsync();
                if (post.IsSub && userId != profileUserId)
                {
                    bool isSub = await _db.UsersPostsSubs.Where(user => user.UserId == userId)
                                                   .Where(p => p.PostId == post.Id)
                                                   .FirstOrDefaultAsync() != null;
                    int price = await _db.PostsPrices.Where(p => p.PostId == post.Id).Select(p => p.Price).FirstOrDefaultAsync();
                    ProfilePostImage profilePostImage = new ProfilePostImage
                    {
                        PostId = post.Id,
                        Created = post.Created,
                        Price = price
                        
                    };

                    if (isSub)
                    {
                        profilePostImage.ImageName = image;
                    }
                    else
                    {
                        profilePostImage.ImageName = string.Empty;

                    }
                    imagePosts.Add(profilePostImage);
                }
                else
                {
                    ProfilePostImage profilePostImage = new ProfilePostImage
                    {
                        PostId = post.Id,
                        ImageName = image,
                        Created = post.Created
                    };
                    imagePosts.Add(profilePostImage);
                }
            }
            imagePosts.Sort((x, y) => DateTime.Compare(x.Created, y.Created));
            imagePosts.Reverse();

            return imagePosts.ToList();
        }

        public async Task<bool> IsFollow(string userId, string followUserId)
        {
            UsersFollow isFollow = await _db.UsersFollows.Where(user => user.UserId == userId)
                                            .Where(user => user.IsFollowingUserId == followUserId)
                                            .FirstOrDefaultAsync();
            return isFollow != null;
        }
    }
}