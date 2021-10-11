using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.OrderAggregate
{
    public sealed class Order : Entity, IAggregateRoot
    {
        public decimal Price { get; }
        public long Quantity { get; }
        public string ProductId { get; }
        public string OwnerId { get; }

        public Order(string ownerId, string productId, decimal price, long quantity)
        {
            OwnerId = ownerId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }
    }
}