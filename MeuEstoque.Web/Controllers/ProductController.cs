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

    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private ApplicationDatabase DB { get; }

        private ILogger Logger { get; }

        public ProductController(ILogger<UserController> logger, ApplicationDatabase db)
        {
            DB = db;
            Logger = logger;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var user = DB.Users
                .Where(user => user.Id == User.FindFirstValue(ClaimTypes.Sid))
                .SingleOrDefault();

            if (user == null)
                return NotFound("User not found");

            var products = DB.Products
                .Where(product => product.OwnerId == user.Id);

            return Ok(products);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(string id)
        {
            var product = DB.Products
                .Where(product => product.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(product => product.Id == id)
                .SingleOrDefault();

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Product> CreateProduct(ProductCreationData data)
        {
            var user = DB.Users
                .Where(user => user.Id == User.FindFirstValue(ClaimTypes.Sid))
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
    }
}
