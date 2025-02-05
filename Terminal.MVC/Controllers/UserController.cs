using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Terminal.Application.Definitions.Requests;
using Terminal.Application.Reports.Requests;
using Terminal.Application.Users;
using Terminal.Application.Votes.Requests;
using Terminal.Domain.Utilities;

namespace Terminal.MVC.Controllers
{
    [Route("/[controller]")]
    [Authorize(Roles = "User, Management")]
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
            if (ModelState.IsValid)
            {
                    await _userService.SuggestDefinition(request, cancellationToken);
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [AutoValidateAntiforgeryToken]
        [Route("/User/ReportDefinition/{id:int}")]
        public async Task<IActionResult> ReportDefinition(int id, ReportCreationRequest request, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await _userService.SendReport(id, request, cancellationToken);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Route("/User/SetVote/{id:int}")]
        public async Task<IActionResult> SetVote(VoteRequestModel vote, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.SetVote(vote, cancellationToken);
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }
        }
    }
}
