using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace xluhco.web.Controllers
{
    [Produces("application/json")]
    [Route("api/Redirect/{shortCode}")]
    public class RedirectController : Controller
    {
        private readonly IShortLinkRepository _shortLinkRepo;

        public RedirectController(IShortLinkRepository shortLinkRepo)
        {
            _shortLinkRepo = shortLinkRepo;
        }
        [HttpGet]
        public IActionResult Index(string shortCode)
        {
            var redirectUrl = _shortLinkRepo.GetByShortCode(shortCode)?.URL;

            if (string.IsNullOrWhiteSpace(redirectUrl))
            {
                return NotFound($"Short link not found for short code {shortCode}");
            }

            return RedirectPermanent(redirectUrl);
        }
    }
}