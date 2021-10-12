using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;
using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.UserAggregate
{
    public sealed class User : Entity, IAggregateRoot
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
        public IEnumerable<Product> Products { get; private set; }

        [JsonIgnore]
        public IEnumerable<Order> Orders { get; private set; }

        public User(string name, string username, string email, string password)
        {
            Name = name;
            Username = username;
            Email = email;
            Password = password;
        }

        private User() {}
    }
}