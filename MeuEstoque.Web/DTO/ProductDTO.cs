using System.ComponentModel.DataAnnotations;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;

namespace MeuEstoque.Web.DTO
{
    public sealed class ProductDTO : EntityDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public ProductDTO(Product product)
        {
            SetEntityValues(product);

            Name = product.Name;
            Description = product.Description;
            ImageUrl = product.ImageUrl;
            Price = product.Price;
            Quantity = product.Quantity;
            OwnerId = product.OwnerId;
        }
    }
}
