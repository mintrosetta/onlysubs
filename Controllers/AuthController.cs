using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OnlySubs.Models.db;
using OnlySubs.Services.PasswordService;
using OnlySubs.Services.UserResourceService;
using OnlySubs.Services.UserRoleService;
using OnlySubs.Services.UserService;
using OnlySubs.ViewModels.Requests;

namespace OnlySubs.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IUserRoleService _userRoleService;

        public AuthController(IUserService userService, 
                              IPasswordService passwordService,
                              IUserRoleService userRoleService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _userRoleService = userRoleService;
        }

        [HttpGet("signin")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("signin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserLoginRequest userRegisterRequest)
        {
            
            if (!ModelState.IsValid) return View(userRegisterRequest);

            User user = await _userService.FindByUsernameAsync(userRegisterRequest.Username);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "Username not found";
                return View();
            }
            if(!_passwordService.Verify(userRegisterRequest.Password, user.Password))
            {
                ViewData["ErrorMessage"] = "Password invalid.";
                return View(userRegisterRequest);
            }

            ClaimsIdentity claims = new ClaimsIdentity(new[] {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, await _userRoleService.GetRoleByUserId(user.Id)),
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(claims);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index","Home");
        }
        [HttpGet("signup")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("signup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterRequest userRegisterRequest)
        {
            ViewData["Title"] = "Sign Up";

            if (!ModelState.IsValid) return View(userRegisterRequest);
            User user = await _userService.FindByUsernameAsync(userRegisterRequest.Username);

            if (user != null)
            {
                ViewData["ErrorMessage"] = "Username is aleady to use.";
                return View(userRegisterRequest);
            }

            await _userService.CreateAsync(userRegisterRequest);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost("signout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet("error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}