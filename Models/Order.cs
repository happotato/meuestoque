using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MeuEstoque.Models
{
    public sealed class Order : DBItem
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [JsonIgnore]
        public User Owner { get; set; }
    }
}