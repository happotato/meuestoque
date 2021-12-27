using System.Linq;
using MeuEstoque.Domain.Services;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;

namespace MeuEstoque.Infrastructure.Services;

public sealed class InventoryService : IInventoryService
{
    ApplicationContext Context { get; }

    public InventoryService(ApplicationContext context)
    {
        Context = context;
    }

    public void AddProduct(Product product)
    {
        Context.Products.Add(product);
        Context.Orders.Add(new Order(product.OwnerId, product.Id, product.Price, product.Quantity));
        Context.SaveChanges();
    }

    public void AddOrder(Order order)
    {
        var product = Context.Products
            .Where(product => product.OwnerId == order.OwnerId)
            .Where(product => product.Id == order.ProductId)
            .SingleOrDefault();

        product.Quantity += order.Quantity;

        Context.Products.Update(product);
        Context.Orders.Add(order);
        Context.SaveChanges();
    }
}