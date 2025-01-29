using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Terminal.Application.Users;
using Terminal.Domain.Models;
using Terminal.MVC.Infrastructure.Auth.JWT;
using Terminal.Application.Users.Requests;
using Terminal.MVC.Models;
using Mapster;

namespace Terminal.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService,
                                ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public Task<List<Claim>> GetClaims(User user)
        {
            return Task.FromResult(
                new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.UserStatus.ToString())
                    });
        }
        public IActionResult SignIn()
        {
            var isAuthenticated = User?.Identity?.IsAuthenticated;
            if (isAuthenticated != null && isAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SignIn([FromForm] UserSignInModel login, CancellationToken cancellationToken)
        {
            var isAuthenticated = User?.Identity?.IsAuthenticated;
            if (isAuthenticated != null && isAuthenticated == true)
            {
                RedirectToAction("Index", "User");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await _userService.AuthenticateAsync(login.Adapt<UserLoginRequest>(), cancellationToken);
                    var claims = await GetClaims(user);

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                    new ClaimsPrincipal(claimsIdentity),
                                                    authProperties);
                    _logger.LogInformation("User {Email} logged in at {Time}.", user.Email, DateTime.UtcNow);
                    return RedirectToAction("Index", "Home");
                }
                catch { }
            }
            return View();
        }

        [AutoValidateAntiforgeryToken]
        [Route("/Auth/Register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterModel registrationRequest, CancellationToken cancellationToken)
        {
            var isAuthenticated = User?.Identity?.IsAuthenticated;
            if (isAuthenticated != null && isAuthenticated == true)
            {
                RedirectToAction("Index", "User");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.RequestRegistration(registrationRequest.Adapt<UserCreationRequest>(), cancellationToken);
                    return RedirectToAction("ConfirmRegistration", "Auth");
                }
                catch { }
            }
            return View();
        }

        [AutoValidateAntiforgeryToken]
        [Route("/Auth/ConfirmRegistration")]
        public async Task<IActionResult> ConfirmRegistration([FromForm] UserConfirmRegistrationModel registrationRequest, CancellationToken cancellationToken)
        {
            var isAuthenticated = User?.Identity?.IsAuthenticated;
            if (isAuthenticated != null && isAuthenticated == true)
            {
                RedirectToAction("Index", "User");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.ConfirmRegistration(registrationRequest.Adapt<UserCreationConfirmationRequest>(), cancellationToken);
                    return RedirectToAction("ConfirmRegistration", "Auth", registrationRequest.Email);
                }
                catch { }
            }
            return View();
        }
    }
}
