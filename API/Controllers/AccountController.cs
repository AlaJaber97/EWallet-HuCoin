using API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<BLL.Models.User> _signInManager;
        private readonly UserManager<BLL.Models.User> _userManager;
        private readonly API.Data.APIContext _context;
        private readonly string UserID;
        public AccountController(
            UserManager<BLL.Models.User> userManager, 
            SignInManager<BLL.Models.User> signInManager,
            IHttpContextAccessor httpContextAccessor,
            API.Data.APIContext context)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._context = context;
            var userclaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userclaim != null) UserID = userclaim.Value;
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
            model.Wallet = new BLL.Models.Wallet();
            model.Wallet.Credential = Utils.RSA.KeyGenerate();
            var result = await _userManager.CreateAsync(model, model.Password);
            if (result.Succeeded)
            {
                return Ok(BLL.Services.JWT.GenerateToken(model));
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest, result.Errors);
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] BLL.Models.ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                    return BadRequest("Can not find user");

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                    return Ok(result);

                return BadRequest(result.Errors);
            }

            return BadRequest("Invalid details");
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromQuery]string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest("Can not find user");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Account", new { token, username = user.UserName }, Request.Scheme);

            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmailPasswordReset(user.Email, link);

            if (emailResponse)
                return Ok("you are send email to your address, please check your email");
            else
            {
                // log email failed 
            }
            return Problem("send email to your address failed");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] BLL.Models.PasswordReset model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                    return BadRequest("Can not find user");

                var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                if (model.NewPassword != model.ConfirmPassword)
                    return BadRequest("The new password and confirm password does not matching");

                var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

                if (result.Succeeded)
                    return Ok(result);

                return BadRequest(result.Errors);
            }

            return BadRequest("Invalid details");
        }

        [HttpGet("Profile")]
        public IActionResult GetProfile()
        {
            var user = _context.Users
                        .Include(user=>user.Wallet)
                        .ThenInclude(wallet=> wallet.Credential)
                        .SingleOrDefault(item => item.Id == UserID);
            if (user != null)
            { 
                return Ok(new BLL.Models.User
                {
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    FamilyName = user.FamilyName,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    UniversityID = user.UniversityID,
                    Image = user.Image,
                    Wallet = user.Wallet
                });
            }
            else return NotFound();
        }

        [HttpPut("Profile")]
        public IActionResult PutProfile([FromBody] BLL.Models.User model)
        {
            var user = _userManager.Users.SingleOrDefault(item => item.Id == UserID);
            if (user != null)
            {
                user.Image = model.Image;
                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;
                user.FamilyName = model.FamilyName;

                var result = _context.SaveChanges();
                if (result > 0)
                    return NoContent();
                else
                    return Content("an error occurred, when try to save update profile");
            }
            else
                return NotFound();
        }
    }
}
