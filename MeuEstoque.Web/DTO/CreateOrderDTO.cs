using System.ComponentModel.DataAnnotations;

namespace MeuEstoque.Web.DTO
{
    public struct CreateOrderDTO
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }

        public string OwnerId { get; set; }
    }
}
