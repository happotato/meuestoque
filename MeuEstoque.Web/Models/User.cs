using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MeuEstoque.Models
{
    public sealed class User : DBItem
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [JsonIgnore]
        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public IEnumerable<Product> Products { get; set; }

        [JsonIgnore]
        public IEnumerable<Order> Orders { get; set; }
    }
}