using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tourism.Controllers.Contracts.Account;
using Tourism.Helper;
using Tourism.Model;

namespace Tourism.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenHelper jwtHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenHelper = jwtHelper;
        }

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            // Generate JWT Token
            if (result.Succeeded)
            {
                var token = _jwtTokenHelper.GenerateJwtToken(user.Id.ToString(), user.UserName, user.Email);

                return Ok(new
                {
                    Token = token,
                    UserId = user.Id // Return userId with the response
                });
            }

            return BadRequest(result.Errors);
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                // Generate JWT Token
                var token = _jwtTokenHelper.GenerateJwtToken(user.Id.ToString(), user.UserName, user.Email);

                return Ok(new
                {
                    Token = token,
                    UserId = user.Id // Return userId with the response
                });
            }

            return Unauthorized(new { Message = "Invalid username or password." });
        }
    }
}
