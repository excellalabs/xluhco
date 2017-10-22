using Microsoft.AspNetCore.Mvc;

namespace xluhco.web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}