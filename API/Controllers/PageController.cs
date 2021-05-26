using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    //locahost:xxxx:/page/PrivacyPolicy
    [Route("[controller]")]
    public class PageController : Controller
    {
        [HttpGet("PrivacyPolicy")]
        public IActionResult PrivacyPolicy()
        {
            ViewData["Title"] = "Privacy Policy";
            return View();
        }
    }
}
