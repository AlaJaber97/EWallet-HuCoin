using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class PageController : Controller
    {
        [HttpGet(nameof(PrivacyPolicy))]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
