using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;

namespace MeuEstoque.Web.Controllers
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
        private IUserRepository UserRepository { get; }

        private IProductRepository ProductRepository { get; }

        private IOrderRepository OrderRepository { get; }

        private ILogger Logger { get; }

        public ProductController(ILogger<UserController> logger,
                               IUserRepository userRepository,
                               IProductRepository productRepository,
                               IOrderRepository orderRepository)
        {
            UserRepository = userRepository;
            ProductRepository = productRepository;
            OrderRepository = orderRepository;
            Logger = logger;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var user = UserRepository.GetById(User.FindFirstValue(ClaimTypes.Sid));

            if (user == null)
                return NotFound("User not found");

            var products = ProductRepository.All
                .Where(product => product.OwnerId == user.Id);

            return Ok(products);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(string id)
        {
            var product = ProductRepository.All
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
            var user = UserRepository.GetById(User.FindFirstValue(ClaimTypes.Sid));

            if (user == null)
                return NotFound("User not found");

            var newProduct = new Product(user.Id, data.Name, data.Description,
                data.ImageUrl, data.Price, data.Quantity);

            var newOrder = new Order(newProduct.OwnerId, newProduct.Id, newProduct.Price, data.Quantity);

            ProductRepository.Add(newProduct);
            OrderRepository.Add(newOrder);

            ProductRepository.Save();
            OrderRepository.Save();

            return GetProductById(newProduct.Id);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public ActionResult<Product> PatchProduct(string id, ProductPatchData data)
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

            return product;
        }
    }
}
