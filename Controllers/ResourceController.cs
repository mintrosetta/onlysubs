using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlySubs.Services.ResourceService;
using OnlySubs.Services.UserResourceService;
using OnlySubs.ViewModels.Requests;

namespace OnlySubs.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ResourceController : Controller
    {
        private readonly IUserResourceService _userResourceService;
        private readonly IResourceService _resourceService;

        public ResourceController(IUserResourceService userResourceService,
                                  IResourceService resourceService)
        {
            _userResourceService = userResourceService;
            _resourceService = resourceService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(RedeemRequest redeemRequest)
        {
            string userId = User.Claims.FirstOrDefault(user => user.Type == "id").Value;
            ViewData["Money"] = await _userResourceService.FindMoney(userId);

            if(!ModelState.IsValid) return View(redeemRequest);

            bool validateCode = _resourceService.ValidateCode(redeemRequest.code);
            Console.WriteLine(validateCode);
            if(!validateCode) 
            {
                Console.WriteLine("Code invalid.");
                ViewData["ErrorMessage"] = "Code invalid.";
                return View(redeemRequest);
            }

            int resourceCount = _resourceService.FindResource(redeemRequest.code);
            await _userResourceService.AddMoney(resourceCount, userId);
            return Redirect("/resource");
        }
    }
}