using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlySubs.Context;
using OnlySubs.Models;
using OnlySubs.Models.db;
using OnlySubs.Services.ImageService;
using OnlySubs.Services.UserService;
using OnlySubs.ViewModels.Requests;
using OnlySubs.ViewModels.Responses;

namespace OnlySubs.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly OnlySubsContext _db;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;

        public PostService(OnlySubsContext db,
                           IUserService userService,
                           IImageService imageService)
        {
            _db = db;
            _userService = userService;
            _imageService = imageService;
        }

        public async Task<string> CreateAsync(PostCreateRequest postCreateRequest, string userId)
        {
            string postId = Guid.NewGuid().ToString();
            bool isSub = false;

            if (postCreateRequest.Price > 0)
            {
                isSub = true;
            }

            Post post = new Post
            {
                Id = postId,
                UserId = userId,
                Description = postCreateRequest.Description,
                IsSub = isSub
            };
            _db.Posts.Add(post);

            if (postCreateRequest.Price > 0)
            {
                PostsPrice price = new PostsPrice
                {
                    PostId = postId,
                    Price = postCreateRequest.Price
                };
                _db.PostsPrices.Add(price);
            }

            List<string> imagesName = new List<string>();
            foreach (IFormFile image in postCreateRequest.Images)
            {
                string imageName = _imageService.Create(image);
                imagesName.Add(imageName);
            }
            foreach (string image in imagesName)
            {
                PostsImage postImage = new PostsImage
                {
                    PostId = postId,
                    ImageName = image,
                };
                _db.PostsImages.Add(postImage);
            }

            await _db.SaveChangesAsync();

            return postId;
        }
        public bool ValidatePostByUserId(string postId, string userId)
        {
            var result = _db.Posts.Where(p => p.Id == postId)
                                  .Where(p => p.UserId == userId)
                                  .FirstOrDefault()
                                  != null;
            return result;
        }
        public async Task<List<PostsResponse>> FindByFollowing(string userId)
        {

            List<string> followingUserId = await _db.UsersFollows.Where(user => user.UserId == userId)
                                                .Select(user => user.IsFollowingUserId).ToListAsync();

            followingUserId.Add(userId);
            List<List<Post>> postByFollowing = new List<List<Post>>();
            foreach (string id in followingUserId)
            {
                List<Post> posts = await _db.Posts.Where(post => post.UserId == id).ToListAsync();
                postByFollowing.Add(posts);
            }

            List<PostsResponse> postsResult = new List<PostsResponse>();
            foreach (List<Post> postsList in postByFollowing)
            {
                foreach (Post item in postsList)
                {
                    User user = await _userService.FindByUserIdAsync(item.UserId);

                    string image = string.Empty;
                    int price = 0;
                    if (item.IsSub && userId != item.UserId)
                    {
                        UsersPostsSub checkSub = await _db.UsersPostsSubs.Where(u => u.UserId == userId)
                                                                         .Where(p => p.PostId == item.Id)
                                                                         .FirstOrDefaultAsync();
                        if (checkSub == null)
                        {
                            price = await _db.PostsPrices.Where(p => p.PostId == item.Id)
                                                         .Select(i => i.Price)
                                                         .FirstOrDefaultAsync();
                        }
                        else 
                        {
                            image = await _db.PostsImages.Where(i => i.PostId == item.Id)
                                                         .Select(u => u.ImageName)
                                                         .FirstOrDefaultAsync();
                        }  
                        
                    }
                    else
                    {
                        image = await _db.PostsImages.Where(i => i.PostId == item.Id)
                                                         .OrderBy(i => i.Created)
                                                         .Select(u => u.ImageName)
                                                         .FirstOrDefaultAsync();
                    }

                    PostsResponse post = new PostsResponse
                    {
                        PostId = item.Id,
                        Username = user.Username,
                        ProfileImage = user.ImageName,
                        ImageName = image,
                        Created = item.Created,
                        Price = price
                    };
                    postsResult.Add(post);
                }
            }

            postsResult.Sort((x, y) => DateTime.Compare(x.Created, y.Created));
            postsResult.Reverse();

            return postsResult;
        }

        public async Task<List<PostsResponse>> FindsByUsername(string username)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            IEnumerable<Post> posts = await _db.Posts.Where(p => p.UserId == user.Id).ToListAsync();

            List<PostsResponse> postsResponses = new List<PostsResponse>();
            foreach (Post post in posts)
            {
                string image = await _db.PostsImages.OrderBy(p => p.Id)
                                                    .Where(p => p.PostId == post.Id)
                                                    .Select(p => p.ImageName)
                                                    .FirstOrDefaultAsync();

                PostsResponse postResponse = new PostsResponse
                {
                    Username = user.Username,
                    ImageName = image,
                    Created = post.Created
                };

                postsResponses.Add(postResponse);
            }

            return postsResponses;
        }

        public async Task<PostResponse> FindByPostId(string postId, string currentUserId)
        {
            Post post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if(post == null)
            {
                PostResponse nullResult = null;
                return nullResult;
            }
            
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == post.UserId);

            List<string> images = new List<string>();
            int price = 0;

            if (post.IsSub && post.UserId != currentUserId)
            {
                UsersPostsSub checkSub = await _db.UsersPostsSubs.Where(u => u.UserId == currentUserId)
                                                                 .Where(p => p.PostId == post.Id)
                                                                 .FirstOrDefaultAsync();
                if (checkSub == null)
                {
                    price = await _db.PostsPrices.Where(p => p.PostId == post.Id)
                                                         .Select(i => i.Price)
                                                         .FirstOrDefaultAsync();
                    images = null;
                }
                else 
                {
                    images =    await _db.PostsImages.Where(i => i.PostId == post.Id)
                                                     .Select(i => i.ImageName)
                                                     .ToListAsync();
                }
            }
            else 
            {
                images =    await _db.PostsImages.Where(i => i.PostId == post.Id)
                                                     .Select(i => i.ImageName)
                                                     .ToListAsync();
            }

            int likesCount = await _db.PostsLikes.Where(l => l.PostId == postId)
                                                 .CountAsync();
            List<PostsComment> commentRaw = await _db.PostsComments.Where(c => c.PostId == post.Id)
                                                      .ToListAsync();
            List<Comment> commentsWarm = new List<Comment>();
            foreach (PostsComment raw in commentRaw)
            {
                User userComment = await _db.Users.FirstOrDefaultAsync(u => u.Id == raw.UserId);
                Comment comment = new Comment
                {
                    Username = userComment.Username,
                    UserImage = userComment.ImageName,
                    Description = raw.Description,
                    Created = raw.Created
                };
                commentsWarm.Add(comment);
            }

            PostResponse postResponse = new PostResponse
            {
                PostId = post.Id,
                UserImage = user.ImageName,
                Username = user.Username,
                Images = images,
                LikesCount = likesCount,
                Comment = commentsWarm,
                Created = post.Created,
                Price = price,
                Description = post.Description
            };
            return postResponse;
        }

        public Task UpdateAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task Remove(string postId)
        {
            List<string> imagesName = await _db.PostsImages.Where(p => p.PostId == postId)
                                                           .Select(p => p.ImageName)
                                                           .ToListAsync();
            var imagesPost = await _db.PostsImages.Where(p => p.PostId == postId).ToListAsync();
            _db.PostsImages.RemoveRange(imagesPost);

            var commentsPost = await _db.PostsComments.Where(p => p.PostId == postId).ToListAsync();
            _db.PostsComments.RemoveRange(commentsPost);

            var likesPost = await _db.PostsLikes.Where(p => p.PostId == postId).ToListAsync();
            _db.PostsLikes.RemoveRange(likesPost);

            var post = await _db.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();
            if(post.IsSub)
            {
                var pricePost = await _db.PostsPrices.Where(p => p.PostId == postId).FirstOrDefaultAsync();
                _db.PostsPrices.Remove(pricePost);

                var usersSub = await _db.UsersPostsSubs.Where(p => p.PostId == postId).ToListAsync();
                _db.UsersPostsSubs.RemoveRange(usersSub);
            }
            _db.Posts.Remove(post);

            await _db.SaveChangesAsync();    

            _imageService.Remove(imagesName);
        }
    }
}