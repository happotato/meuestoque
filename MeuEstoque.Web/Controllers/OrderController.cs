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
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private ApplicationDatabase DB { get; }

        private ILogger Logger { get; }

        public OrderController(ILogger<UserController> logger, ApplicationDatabase db)
        {
            DB = db;
            Logger = logger;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            var user = DB.Users
                .Where(user => user.Id == User.FindFirstValue(ClaimTypes.Sid))
                .SingleOrDefault();

            if (user == null)
                return NotFound("User not found");

            var orders = DB.Orders
                .Include(order => order.Product)
                .Where(order => order.OwnerId == user.Id);

            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(string id)
        {
            var order = DB.Orders
                .Include(order => order.Product)
                .Where(order => order.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(order => order.Id == id)
                .SingleOrDefault();

            if (order == null)
                return NotFound("Order not found");

            return Ok(order);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Order> CreateOrder(OrderCreationData data)
        {
             var product = DB.Products
                .Where(product => product.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(product => product.Id == data.ProductId)
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
