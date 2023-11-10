using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlySubs.Models;
using OnlySubs.Services.PostService;
using OnlySubs.Services.UserResourceService;
using OnlySubs.ViewModels.Responses;

namespace OnlySubs.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserResourceService _userResourceService;
        private readonly IPostService _postService;

        public HomeController(ILogger<HomeController> logger, 
                              IUserResourceService userResourceService,
                              IPostService postService)
        {
            _logger = logger;
            _userResourceService = userResourceService;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);

            List<PostsResponse> result = await _postService.FindByFollowing(userId);
            if(result.Count == 0)
            {
                result = null;
            }
            return View(result);
        }

        public async Task<IActionResult> Privacy()
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);
            return View();
        }
        [HttpGet("about")]
        public async Task<IActionResult> AboutUs()
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
