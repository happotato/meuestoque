using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MeuEstoque.Data;
using MeuEstoque.Models;
using Microsoft.EntityFrameworkCore;

namespace MeuEstoque.Controllers
{
    public struct UserCreationData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(24)]
        public string Password { get; set; }
    }

    public struct UserLoginData
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private ApplicationDatabase DB { get; }

        private IUserStore<User> UserStore { get; }

        private ILogger Logger { get; }

        public UserController(ILogger<UserController> logger, ApplicationDatabase db, IUserStore<User> userStore)
        {
            DB = db;
            UserStore = userStore;
            Logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var user = await UserStore.FindByIdAsync(User.FindFirstValue(ClaimTypes.Sid), CancellationToken.None);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser(UserCreationData data)
        {
            var user = new User
            {
                Name = data.Name,
                Username = data.Username,
                Email = data.Email,
                Password = data.Password,
            };

            var duplicated = DB.Users
                .Where(user => user.Email == data.Email || user.Username == data.Username)
                .Count();

            if (duplicated > 0) {
                return UnprocessableEntity("Username or Email already in use");
            }

            await UserStore.CreateAsync(user, CancellationToken.None);

            return await Login(new UserLoginData
            {
                Email = user.Email,
                Password = user.Password,
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLoginData data)
        {
            var user = DB.Users
                .Where(user => user.Email == data.Email && user.Password == data.Password)
                .SingleOrDefault();

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
            };

            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var properties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            };

            await HttpContext.SignInAsync(scheme, claimsPrincipal, properties);

            return Ok(user);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
