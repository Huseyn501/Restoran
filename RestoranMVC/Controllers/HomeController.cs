using Microsoft.AspNetCore.Mvc;

namespace RestoranMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
