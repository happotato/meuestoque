using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;
using MeuEstoque.Domain.Services;
using MeuEstoque.Web.DTO;

namespace MeuEstoque.Web.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private IUserRepository UserRepository { get; }

        private IProductRepository ProductRepository { get; }

        private IOrderRepository OrderRepository { get; }

        private IInventoryService InventoryService { get; }

        private ILogger Logger { get; }

        public ProductController(ILogger<UserController> logger,
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
        public ActionResult<IEnumerable<ProductDTO>> GetProducts()
        {
            var user = UserRepository.GetById(User.FindFirstValue(ClaimTypes.Sid));

            if (user == null)
                return NotFound("User not found");

            var products = ProductRepository.All
                .Where(product => product.OwnerId == user.Id)
                .Select(product => new ProductDTO(product))
                .ToList();

            return Ok(products);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<ProductDTO> GetProductById(string id)
        {
            var product = ProductRepository.All
                .Where(product => product.OwnerId == User.FindFirstValue(ClaimTypes.Sid))
                .Where(product => product.Id == id)
                .SingleOrDefault();

            if (product == null)
                return NotFound("Product not found");

            return new ProductDTO(product);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<ProductDTO> CreateProduct(CreateProductDTO data)
        {
            var user = UserRepository.GetById(User.FindFirstValue(ClaimTypes.Sid));

            if (user == null)
                return NotFound("User not found");

            var product = new Product(user.Id, data.Name, data.Description,
                data.ImageUrl, data.Price, data.Quantity);

            InventoryService.AddProduct(product);
            return new ProductDTO(product);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public ActionResult<ProductDTO> PatchProduct(string id, PatchProductDTO data)
        {
            var user = UserRepository.GetById(User.FindFirstValue(ClaimTypes.Sid));

            if (user == null)
                return NotFound("User not found");

            var product = ProductRepository.GetById(id);

            product.Name = data.Name;
            product.ImageUrl = data.ImageUrl;
            product.Price = data.Price;
            product.Description = data.Description;

            ProductRepository.Update(product);
            ProductRepository.Save();

            return new ProductDTO(product);
        }
    }
}
