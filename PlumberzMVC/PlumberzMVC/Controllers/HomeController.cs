using Microsoft.AspNetCore.Mvc;

namespace PlumberzMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
