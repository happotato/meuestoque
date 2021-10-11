using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.ProductAggregate
{
    public sealed class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public long Quantity { get; set; }
        public string OwnerId { get; }

        public Product(string ownerId, string name, string desc, string imageUrl, decimal price)
        {
            OwnerId = ownerId;
            Name = name;
            Description = desc;
            ImageUrl = imageUrl;
            Price = price;
        }
    }
}