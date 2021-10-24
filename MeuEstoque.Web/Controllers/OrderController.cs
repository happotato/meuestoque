using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;
using MeuEstoque.Domain.Services;

namespace MeuEstoque.Web.Controllers
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
        private IUserRepository UserRepository { get; }

        private IProductRepository ProductRepository { get; }

        private IOrderRepository OrderRepository { get; }

        private IInventoryService InventoryService { get; }

        private ILogger Logger { get; }

        public OrderController(ILogger<UserController> logger,
                               IUserRepository userRepository,
                               IProductRepository productRepository,
                               IOrderRepository orderRepository,
                               IInventoryService inventoryService)
        {
            UserRepository = userRepository;
            ProductRepository = productRepository;
            OrderRepository = orderRepository;
            InventoryService = inventoryService;
            Logger = logger;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            var user = UserRepository.GetById(User.FindFirstValue(ClaimTypes.Sid));

            if (user == null)
                return NotFound("User not found");

            var orders = OrderRepository.All
                .Include(order => order.Product)
                .Where(order => order.OwnerId == user.Id);

            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(string id)
        {
            var order = OrderRepository.All
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
            var exists = ProductRepository.All
                .Where(product => product.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(product => product.Id == data.ProductId)
                .Any();

            if (!exists)
                return NotFound("Product not found");

            var order = new Order(data.OwnerId, data.ProductId, data.Price, data.Quantity);
            InventoryService.AddOrder(order);

            return order;
        }
    }
}
