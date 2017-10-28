using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace xluhco.web.Controllers
{
    public class HomeController : Controller
    {
        private IShortLinkRepository _repo;
        private SiteOptions _siteOptions;

        public HomeController(IShortLinkRepository repo, IOptions<SiteOptions> siteOptions)
        {
            _repo = repo;
            _siteOptions = siteOptions.Value;
        }

        public IActionResult Index()
        {
            return View(_siteOptions);
        }

        public IActionResult List()
        {
            var orderedLinks = _repo.GetShortLinks().OrderBy(x => x.ShortLinkCode).ToList();
            return View(orderedLinks);
        }
    }
}