using CW_W16_01_114.Models;
using DataAccess;
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
            entity.Add("BookId", Guid.NewGuid());
            entity.Add("LastName", "mohammadi");

            var userRepo = new DataAccess.UserRepository("Server=.\\SQLEXPRESS;Database=UserTest;" + "Integrated Security=true");
            
            var users = new List<User>() {
                new DataAccess.User() {Id = Guid.NewGuid(), Name = "ali", LastName = "alian" },
                new DataAccess.User() {Id = Guid.NewGuid(), Name = "saeed", LastName = "alian" },
                new DataAccess.User() {Id = Guid.NewGuid(), Name = "vali", LastName = "alian" },
                new DataAccess.User() {Id = Guid.NewGuid(), Name = "asghar", LastName = "alian" }

            };
            userRepo.WriteAll(users);
            var user = userRepo.GetByIdWhitBooks(users.First().Id);
            var bookRepo = new BookRepository("Server=.\\SQLEXPRESS;Database=UserTest;" + "Integrated Security=true");
            var book = bookRepo.GetById(2);
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
