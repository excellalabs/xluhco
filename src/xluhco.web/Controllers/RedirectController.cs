using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace xluhco.web.Controllers
{
    [Produces("application/json")]
    [Route("api/Redirect/{shortCode}")]
    public class RedirectController : Controller
    {
        [HttpGet]
        public IActionResult Index(string shortCode)
        {
            return Ok($"Hello there! Redirecting for short code {shortCode}");
        }
    }
}