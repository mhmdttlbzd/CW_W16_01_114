using CW_W16_01_114.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CW_W16_01_114.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var entity = new Dictionary<string, object>();
            entity.Add("Name", "mohammad");
            entity.Add("Id", Guid.NewGuid());
            entity.Add("LastName", "mohammadi");

            var userRepo = new DataAccess.UserRepository("Server=.\\SQLEXPRESS;Database=UserTest;" + "Integrated Security=true");
            var users = userRepo.GetAll();
            var user = userRepo.GetByIdWhitBooks(users.First().Id);
            return View();
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
