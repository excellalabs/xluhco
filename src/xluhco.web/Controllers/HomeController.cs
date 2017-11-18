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
            if(repo == null) { throw new ArgumentNullException(nameof(repo));}
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