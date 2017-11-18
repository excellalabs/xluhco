using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace xluhco.web.Controllers
{
    public class HomeController : Controller
    {
        private IShortLinkRepository _repo;

        public HomeController(IShortLinkRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            var orderedLinks = _repo.GetShortLinks().OrderBy(x => x.ShortLinkCode).ToList();
            return View(orderedLinks);
        }
    }
}