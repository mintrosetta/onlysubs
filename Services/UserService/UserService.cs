using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlySubs.Context;
using OnlySubs.Models.db;
using OnlySubs.Services.ImageService;
using OnlySubs.Services.PasswordService;
using OnlySubs.Services.UserResourceService;
using OnlySubs.Services.UserRoleService;
using OnlySubs.ViewModels.Requests;

namespace OnlySubs.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly OnlySubsContext _db;
        private readonly IUserRoleService _userRoleService;
        private readonly IPasswordService _passwordService;
        private readonly IUserResourceService _userResourceService;
        private readonly IImageService _imageService;

        public UserService(OnlySubsContext db,
                           IUserRoleService userRoleService,
                           IPasswordService passwordService,
                           IUserResourceService userResourceService,
                           IImageService imageService)
        {
            _db = db;
            _userRoleService = userRoleService;
            _passwordService = passwordService;
            _userResourceService = userResourceService;
            _imageService = imageService;
        }

        public async Task CreateAsync(UserRegisterRequest userRegisterRequest)
        {
            string userId = Guid.NewGuid().ToString();
            User user = new User
            {
                Id = userId,
                Username = userRegisterRequest.Username,
                Password = _passwordService.Hash(userRegisterRequest.Password),
                ImageName = "nopic.jpg"
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            await _userRoleService.AddRoleByUserIdAsync(userId, 1);
            await _userResourceService.SetResource(0, userId);
        }

        public async Task<User> FindByUserIdAsync(string userId)
        {
            return await _db.Users.FirstOrDefaultAsync(user => user.Id == userId);
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task UpdateAsync(UserUpdateRequest userUpdateRequest,string userId)
        {
            User user = await FindByUserIdAsync(userId);
            user.Username = userUpdateRequest.Username;

            if(!string.IsNullOrEmpty(userUpdateRequest.Description)) user.Description = userUpdateRequest.Description;
            
            if(userUpdateRequest.ImageProfile != null) user.ImageName = _imageService.Create(userUpdateRequest.ImageProfile);

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            Console.WriteLine("Update complete");
        }

        public async Task RemoveAsync(string userId)
        {
            User user = await FindByUserIdAsync(userId);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<int> FindFollowerCount(string userId)
        {
            return await _db.UsersFollows.Where(user => user.IsFollowingUserId == userId)
                                         .Select(fol => fol.IsFollowingUserId)
                                         .CountAsync();
        }

        public async Task<int> FindFollowingCount(string userId)
        {
            return await _db.UsersFollows.Where(user => user.UserId == userId)
                                         .Select(fol => fol.UserId)
                                         .CountAsync();
        }

        public async Task FollowToggle(string userId, string FollowUserId)
        {
            UsersFollow isFollow = await _db.UsersFollows.Where(u => u.UserId == userId)
                                                         .Where(u => u.IsFollowingUserId == FollowUserId)
                                                         .FirstOrDefaultAsync();
            if (isFollow != null)
            {
                _db.UsersFollows.Remove(isFollow);
            }
            else
            {
                UsersFollow usersFollow = new UsersFollow
                {
                    UserId = userId,
                    IsFollowingUserId = FollowUserId
                };
                _db.UsersFollows.Add(usersFollow);
            }
            await _db.SaveChangesAsync();
        }
    }
}