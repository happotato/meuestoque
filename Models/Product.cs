using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace MeuEstoque.Models
{

    public sealed class Product : DBItem
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

        [JsonIgnore]
        public User Owner { get; set; }

        [JsonIgnore]
        public IEnumerable<Order> Orders { get; set; }
    }
}