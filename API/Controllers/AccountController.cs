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
    public class AccountController : Controller
    {
        private readonly SignInManager<BLL.Models.User> _signInManager;
        private readonly UserManager<BLL.Models.User> _userManager;
        private readonly API.Services.EmailSender _emailSender;
        private readonly API.Data.APIContext _context;
        private readonly string UserID;
        public AccountController(
            UserManager<BLL.Models.User> userManager, 
            SignInManager<BLL.Models.User> signInManager,
            API.Services.EmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            API.Data.APIContext context)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._emailSender = emailSender;
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
                if (result.IsLockedOut) return Problem("your account is locked out");
                if (result.IsNotAllowed) return Problem("your account is not allowed");
                if (result.RequiresTwoFactor) return Problem("your account is requires two factor");
                return Unauthorized("username or password not correct");
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
                string ErrorMessage = string.Empty;
                foreach (var error in result.Errors)
                    ErrorMessage += $"\n• {error.Description}";
                return Problem(ErrorMessage);
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] BLL.Models.ChangePassword model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) return NotFound($"Can not found user {model.UserName}");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded) return Ok();

            string ErrorMessage = string.Empty;
            foreach (var error in result.Errors)
                ErrorMessage += $"\n• {error.Description}";
            return Problem(ErrorMessage);
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

        [AllowAnonymous]
        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromForm] BLL.Models.ForgotPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null) return RedirectToAction(nameof(ForgotPasswordFailed));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme, BLL.Settings.Connections.GetServerAddress());
            var callback = $"{BLL.Settings.Connections.GetServerAddress()}/{Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email })}";
            var message = new Services.Message(user.Email, "Reset Password", callback);
            await _emailSender.SendEmailAsync(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }
        [AllowAnonymous]
        [HttpGet("ForgotPasswordConfirmation")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet("ForgotPasswordFailed")]
        public IActionResult ForgotPasswordFailed()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string token, string email)
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] BLL.Models.ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null) return RedirectToAction(nameof(ResetPasswordFailed));

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded) return RedirectToAction(nameof(ResetPasswordConfirmation));

            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);

            }
            return View();
        }
        [AllowAnonymous]
        [HttpGet("ResetPasswordConfirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet("ResetPasswordFailed")]
        public IActionResult ResetPasswordFailed()
        {
            return View();
        }
    }
}
