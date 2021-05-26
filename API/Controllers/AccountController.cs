using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
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
        private readonly EmailService.IEmailSender _emailSender;
        private readonly API.Data.APIContext _context;
        private readonly string UserID;
        public AccountController(
            UserManager<BLL.Models.User> userManager, 
            SignInManager<BLL.Models.User> signInManager,
            EmailService.IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            API.Data.APIContext context)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._context = context;
            this._emailSender = emailSender;
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
                return Problem(string.Join("\n• ", result.Errors));
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] BLL.Models.ChangePassword model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) return NotFound($"Can not found user {model.UserName}");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded) return Ok();

            return Problem(string.Join("\n• ", result.Errors));
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

        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromForm] BLL.Models.ForgotPassword forgotPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            var message = new EmailService.Message(new string[] { user.Email }, "Reset password token", callback, null);
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
        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new BLL.Models.ResetPassword { Token = token, Email = email };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] BLL.Models.ResetPassword resetPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [AllowAnonymous]
        [HttpGet("ResetPasswordConfirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
