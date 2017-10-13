using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace xluhco.web.Controllers
{
    [Produces("application/json")]
    [Route("api/Redirect")]
    public class RedirectController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Hello there!");
        }
    }
}