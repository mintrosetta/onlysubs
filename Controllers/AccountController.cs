using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlySubs.Models;
using OnlySubs.Models.db;
using OnlySubs.Services.ImageService;
using OnlySubs.Services.PostService;
using OnlySubs.Services.ProfileService;
using OnlySubs.Services.SearchService;
using OnlySubs.Services.UserResourceService;
using OnlySubs.Services.UserService;
using OnlySubs.ViewModels.Requests;
using OnlySubs.ViewModels.Responses;

namespace OnlySubs.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IUserResourceService _userResourceService;
        private readonly IProfileService _profileService;
        private readonly IImageService _imageService;
        private readonly ISearchService _searchService;

        public AccountController(IUserService userService,
                                 IPostService postService,
                                 IUserResourceService userResourceService,
                                 IProfileService profileService,
                                 IImageService imageService,
                                 ISearchService searchService)
        {
            _userService = userService;
            _postService = postService;
            _userResourceService = userResourceService;
            _profileService = profileService;
            _imageService = imageService;
            _searchService = searchService;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Index(string username)
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);

            User user = await _userService.FindByUsernameAsync(username);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "Account not found.";
                return View();
            }

            var result = await _profileService.FindDetail(userId, user.Id);

            ViewData["isFollow"] = await _profileService.IsFollow(userId, user.Id);

            return View(result);
        }
        [HttpGet("setting")]
        public async Task<IActionResult> Setting()
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);

            User user = await _userService.FindByUserIdAsync(userId);
            UserDefaultSetting userDefault = new UserDefaultSetting
            {
                ImageName = user.ImageName,
                Username = user.Username,
                Description = user.Description
            };
            ViewData["DefaultSetting"] = userDefault;
            return View();
        }
        [HttpPost("setting")]
        public async Task<IActionResult> Setting(UserUpdateRequest userUpdateRequest)
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);

            User users = await _userService.FindByUserIdAsync(userId);
            UserDefaultSetting userDefault = new UserDefaultSetting
            {
                ImageName = users.ImageName,
                Username = users.Username,
                Description = users.Description
            };
            ViewData["DefaultSetting"] = userDefault;

            if (!ModelState.IsValid) return View(userUpdateRequest);

            User user = await _userService.FindByUsernameAsync(userUpdateRequest.Username);
            bool reset = false;
            if (user != null)
            {
                if (userId != user.Id)
                {
                    Console.WriteLine("Username is aleady to use.");

                    ViewData["ErrorMessage"] = "Username is aleady to use.";
                    return View(userUpdateRequest);
                }
                else 
                {
                    reset = true;
                }
            }

            
            if (userUpdateRequest.ImageProfile != null)
            {
                string[] extention = new string[] { ".jpg", ".png" };
                if (!_imageService.ValidateExtension(userUpdateRequest.ImageProfile, extention))
                {
                    Console.WriteLine("Image cant upload.");

                    ViewData["ErrorMessage"] = "Please upload an image with a .jpg , .png file extension.";
                    return View(userUpdateRequest);
                }
            }


            await _userService.UpdateAsync(userUpdateRequest, userId);

            if(!reset) 
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect("/");
            }
            return Redirect("/account/setting");
        }
        [HttpPost("follow/{username}")]
        public async Task<IActionResult> FollowToggle(string username)
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            User user = await _userService.FindByUsernameAsync(username);

            if (user == null) return Redirect($"/account/{username}");

            await _userService.FollowToggle(userId, user.Id);
            return Redirect($"/account/{username}");
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(string search)
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);
            
            var result = await _searchService.Finds(search);
            if(result.Count <= 0) result = null;
            
            return View(result);
        }
    }
}