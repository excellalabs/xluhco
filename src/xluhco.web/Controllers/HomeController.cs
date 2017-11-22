using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using xluhco.web.Repositories;

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
            return View("Index");
        }

        public IActionResult List()
        {
            var orderedLinks = _repo.GetShortLinks().OrderBy(x => x.ShortLinkCode).ToList();
            return View("List", orderedLinks);
        }
    }
}