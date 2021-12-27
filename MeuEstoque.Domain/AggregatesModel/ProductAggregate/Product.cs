using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;
using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.ProductAggregate;

public sealed class Product : Entity, IAggregateRoot
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
    public string OwnerId { get; private set; }

    [JsonIgnore]
    public User Owner { get; private set; }

    [JsonIgnore]
    public IEnumerable<Order> Orders { get; private set; }

    public Product(string ownerId, string name, string desc, string imageUrl, decimal price, long quantity)
    {
        OwnerId = ownerId;
        Name = name;
        Description = desc;
        ImageUrl = imageUrl;
        Price = price;
        Quantity = quantity;
    }

    private Product() {}
}