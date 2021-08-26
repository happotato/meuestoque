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

    public struct ProductCreationData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }
    }

    public struct ProductPatchData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public decimal Price { get; set; }
    }

    public struct OrderCreationData
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }

        public string OwnerId { get; set; }
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
                .ToList()
                .DefaultIfEmpty(null)
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

        [Authorize]
        [HttpGet("products")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var user = DB.Users
                .Where(user => user.Id == User.FindFirstValue(ClaimTypes.Sid))
                .ToList()
                .DefaultIfEmpty(null)
                .SingleOrDefault();

            if (user == null)
                return NotFound("User not found");

            var products = DB.Products
                .Where(product => product.OwnerId == user.Id);

            return Ok(products);
        }

        [Authorize]
        [HttpGet("products/{id}")]
        public ActionResult<Product> GetProductById(string id)
        {
            var product = DB.Products
                .Where(product => product.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(product => product.Id == id)
                .ToList()
                .DefaultIfEmpty(null)
                .SingleOrDefault();

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [Authorize]
        [HttpPost("products")]
        public ActionResult<Product> CreateProduct(ProductCreationData data)
        {
            var user = DB.Users
                .Where(user => user.Id == User.FindFirstValue(ClaimTypes.Sid))
                .ToList()
                .DefaultIfEmpty(null)
                .SingleOrDefault();

            if (user == null)
                return NotFound("User not found");

            var newProduct = new Product
            {
                Name = data.Name,
                Description = data.Description,
                ImageUrl = data.ImageUrl,
                Quantity = data.Quantity,
                Price = data.Price,
                Owner = user,
                OwnerId = user.Id,
            };

            var newOrder = new Order
            {
                Price = newProduct.Price,
                Quantity = data.Quantity,
                ProductId = newProduct.Id,
                OwnerId = newProduct.OwnerId,
            };

            DB.Products.Add(newProduct);
            DB.Orders.Add(newOrder);
            DB.SaveChanges();

            return GetProductById(newProduct.Id);
        }

        [Authorize]
        [HttpPatch("products/{id}")]
        public ActionResult<Product> PatchProduct(string id, ProductPatchData data)
        {
            var product = DB.Products
                .Where(product => product.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(product => product.Id == id)
                .ToList()
                .DefaultIfEmpty(null)
                .SingleOrDefault();

            if (product == null)
                return NotFound("Product not found");

            product.Name = data.Name;
            product.Description = data.Description;
            product.ImageUrl = data.ImageUrl;
            product.Price = data.Price;

            DB.Products.Update(product);
            DB.SaveChanges();

            return Ok(product);
        }

        [Authorize]
        [HttpGet("orders")]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            var user = DB.Users
                .Where(user => user.Id == User.FindFirstValue(ClaimTypes.Sid))
                .ToList()
                .DefaultIfEmpty(null)
                .SingleOrDefault();

            if (user == null)
                return NotFound("User not found");

            var orders = DB.Orders
                .Include(order => order.Product)
                .Where(order => order.OwnerId == user.Id);

            return Ok(orders);
        }

        [Authorize]
        [HttpGet("orders/{id}")]
        public ActionResult<Order> GetOrderById(string id)
        {
            var order = DB.Orders
                .Include(order => order.Product)
                .Where(order => order.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(order => order.Id == id)
                .ToList()
                .DefaultIfEmpty(null)
                .SingleOrDefault();

            if (order == null)
                return NotFound("Order not found");

            return Ok(order);
        }

        [Authorize]
        [HttpPost("orders")]
        public ActionResult<Order> CreateOrder(OrderCreationData data)
        {
             var product = DB.Products
                .Where(product => product.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(product => product.Id == data.ProductId)
                .ToList()
                .DefaultIfEmpty(null)
                .SingleOrDefault();

            if (product == null)
                return NotFound("Product not found");

            var newOrder = new Order
            {
                Price = data.Price,
                Quantity = data.Quantity,
                ProductId = data.ProductId,
                OwnerId = data.OwnerId,
            };

            product.Quantity += newOrder.Quantity;

            DB.Products.Update(product);
            DB.Orders.Add(newOrder);
            DB.SaveChanges();

            return newOrder;
        }
    }
}
