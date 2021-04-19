using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<BLL.Models.User> _signInManager;
        private readonly UserManager<BLL.Models.User> _userManager;
        private readonly API.Data.APIContext _context;
        public AccountController(UserManager<BLL.Models.User> userManager, SignInManager<BLL.Models.User> signInManager, API.Data.APIContext context)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._context = context;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] BLL.Models.RequestLogin requestLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(requestLogin.Username, requestLogin.Password, false, false);
            if(result.Succeeded)
            {
                var user = _userManager.Users.Single(user => user.UserName == requestLogin.Username);
                return Ok(BLL.Services.JWT.GenerateToken(user));
            }
            else
            {
                if (result.IsLockedOut) return StatusCode((int)HttpStatusCode.UnavailableForLegalReasons, "your account is locked out");
                if (result.IsNotAllowed) return StatusCode((int)HttpStatusCode.UnavailableForLegalReasons, "your account is not allowed");
                if (result.RequiresTwoFactor) return StatusCode((int)HttpStatusCode.UnavailableForLegalReasons, "your account is requires two factor");
                return StatusCode((int)HttpStatusCode.Unauthorized, "username or password not correct");
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] BLL.Models.User model)
        {
            var result = await _userManager.CreateAsync(model, model.Password);
            if (result.Succeeded)
            {
                model.Password = string.Empty;
                return Ok(BLL.Services.JWT.GenerateToken(model));
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest, result.Errors);
            }
        }
    }
}
