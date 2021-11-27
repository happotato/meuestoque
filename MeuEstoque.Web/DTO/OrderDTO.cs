using System.ComponentModel.DataAnnotations;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;

namespace MeuEstoque.Web.DTO
{
    public sealed class OrderDTO : EntityDTO
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public ProductDTO Product { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public OrderDTO(Order order)
        {
            SetEntityValues(order);

            Price = order.Price;
            Quantity = order.Quantity;
            ProductId = order.ProductId;
            Product = new ProductDTO(order.Product);
            OwnerId = order.OwnerId;
        }
    }
}
