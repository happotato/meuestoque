using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;
using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.OrderAggregate
{
    public sealed class Order : Entity, IAggregateRoot
    {
        [Required]
        public decimal Price { get; private set; }

        [Required]
        public long Quantity { get; private set; }

        [Required]
        public string ProductId { get; private set; }

        [Required]
        public Product Product { get; private set; }

        [Required]
        public string OwnerId { get; private set; }

        [JsonIgnore]
        public User Owner { get; private set; }

        public Order(string ownerId, string productId, decimal price, long quantity)
        {
            OwnerId = ownerId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }

        private Order() {}
    }
}