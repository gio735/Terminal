using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Terminal.Application.Definitions.Requests;
using Terminal.Application.Users;

namespace Terminal.MVC.Controllers
{
    [Route("/[controller]")]
    [Authorize(Roles = "User, Manager")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public UserController(IUserService userService,
                                ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AutoValidateAntiforgeryToken]
        [Route("/User/SuggestDefinition")]
        public async Task<IActionResult> SuggestDefinition(DefinitionCreationRequest request, CancellationToken cancellationToken)
        {

        }
    }
}
