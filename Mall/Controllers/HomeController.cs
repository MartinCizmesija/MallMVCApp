using Mall.Models;
using Mall.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mall.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeRepository _repository;

        public HomeController( HomeRepository repository)
        {
            _repository = repository;
        }

        public  IActionResult Index()
        {
            var mall =  _repository.Get(1);
            return View(mall);
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
