using System.Linq;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;

namespace MeuEstoque.Infrastructure.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private ApplicationContext Context { get; }

    public IQueryable<Order> All => Context.Orders;

    public OrderRepository(ApplicationContext context)
    {
        Context = context;
    }

    public Order Add(Order obj)
    {
        var updatedOrder = Context.Orders.Add(obj).Entity;
        Context.SaveChanges();

        return updatedOrder;
    }

    public Order GetById(string id)
    {
        return Context.Orders
            .SingleOrDefault(order => order.Id == id);
    }
    
    public Order Update(Order obj)
    {
        var updatedOrder = Context.Orders.Update(obj).Entity;
        Context.SaveChanges();

        return updatedOrder;
    }

    public void Remove(Order obj)
    {
        Context.Orders.Remove(obj);
        Context.SaveChanges();
    }
}