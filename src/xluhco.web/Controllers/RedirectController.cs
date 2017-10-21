using Microsoft.AspNetCore.Mvc;

namespace xluhco.web.Controllers
{
    [Produces("application/json")]
    [Route("api/Redirect/{shortCode}")]
    public class RedirectController : Controller
    {
        private readonly IShortLinkRepository _shortLinkRepo;
        private readonly Serilog.ILogger _logger;

        public RedirectController(IShortLinkRepository shortLinkRepo, Serilog.ILogger logger)
        {
            _shortLinkRepo = shortLinkRepo;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index(string shortCode)
        {
            _logger.Debug("Entered the redirect for short code {shortCode}", shortCode);
            var redirectItem = _shortLinkRepo.GetByShortCode(shortCode);

            if (string.IsNullOrWhiteSpace(redirectItem?.URL))
            {
                _logger.Warning("No redirect found for requested short code {shortCode}", shortCode);
                return NotFound($"Short link not found for short code '{shortCode}'");
            }

            _logger.Information("Redirecteing {shortCode} to {redirectUrl}", shortCode, redirectItem);

            return View("Index", redirectItem);
            //return RedirectPermanent(redirectItem);
        }
    }
}