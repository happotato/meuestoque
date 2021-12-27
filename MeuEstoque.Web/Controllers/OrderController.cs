using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;
using MeuEstoque.Domain.Services;
using MeuEstoque.Web.DTO;

namespace MeuEstoque.Web.Controllers;

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
            .Where(order => order.OwnerId == user.Id)
            .Select(order => new OrderDTO(order));

        return Ok(orders);
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<OrderDTO> GetOrderById(string id)
    {
        var order = OrderRepository.All
            .Include(order => order.Product)
            .Where(order => order.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
            .Where(order => order.Id == id)
            .SingleOrDefault();

        if (order == null)
            return NotFound("Order not found");

        return new OrderDTO(order);
    }

    [Authorize]
    [HttpPost]
    public ActionResult<OrderDTO> CreateOrder(CreateOrderDTO data)
    {
        var exists = ProductRepository.All
            .Where(product => product.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
            .Where(product => product.Id == data.ProductId)
            .Any();

        if (!exists)
            return NotFound("Product not found");

        var order = new Order(data.OwnerId, data.ProductId, data.Price, data.Quantity);
        InventoryService.AddOrder(order);

        return new OrderDTO(order);
    }
}
