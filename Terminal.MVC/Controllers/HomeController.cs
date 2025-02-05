using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Terminal.Application.Definitions.Responses;
using Terminal.Application.Users;
using Terminal.MVC.Models;

namespace Terminal.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUserService userService,
                                ILogger<HomeController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await _userService.GetRandomDefinitionsForMainPage(cancellationToken);
            List<(DefinitionResponseModel, bool?)> response = new();
            response.Add((new()
            {
                Id = 1,
                GeorgianTitle = "პირველი",
                GeorgianContent = "პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი.",
                EnglishTitle = "First",
                EnglishContent = "First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text.",
                UpvoteCount= 142,
                DownvoteCount = 40,
                References = new(),
                Similars = new()
            }, true));
            
            response.Add((new()
            {
                Id = 2,
                GeorgianTitle = "მეორე",
                GeorgianContent = "პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი. პირველი ტექსტი.",
                EnglishTitle = "Second",
                EnglishContent = "First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text. First text.",
                UpvoteCount = 100,
                DownvoteCount = 100,
                References = new(),
                Similars = new()
            }, false));
            response[0].Item1.References.Add(new()
            {
                Id = 1,
                ReferenceId = 1,
                GeorgianTitle = "პირველი",
                EnglishTitle = "First"
            });
            response[0].Item1.References.Add(new()
            {
                Id = 2,
                ReferenceId = 2,
                GeorgianTitle = "მეორე",
                EnglishTitle = "Second"
            });
            response[1].Item1.References.Add(new()
            {
                Id = 1,
                ReferenceId = 1,
                GeorgianTitle = "პირველი",
                EnglishTitle = "First"
            });
            response[1].Item1.References.Add(new()
            {
                Id = 2,
                ReferenceId = 2,
                GeorgianTitle = "მეორე",
                EnglishTitle = "Second"
            });
            response[0].Item1.Similars.Add(response[1].Item1);
            response[1].Item1.Similars.Add(response[0].Item1);
            return View(response);
        }
        [Route("/Home/Definition/{id:int}")]
        public async Task<IActionResult> GetDefinition(int id, CancellationToken cancellationToken)
        {
            return View(await _userService.GetDefinition(id,cancellationToken));
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
