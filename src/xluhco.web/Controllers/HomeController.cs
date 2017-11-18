using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace xluhco.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShortLinkRepository _repo;

        public HomeController(IShortLinkRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
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