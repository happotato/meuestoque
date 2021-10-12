using System.Linq;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;

namespace MeuEstoque.Infrastructure.Repositories
{
    public sealed class OrderRepository : IOrderRepository
    {
        ApplicationContext Context { get; }

        public IQueryable<Order> All => Context.Orders;

        public OrderRepository(ApplicationContext context)
        {
            Context = context;
        }

        public Order Add(Order obj)
        {
            return Context.Orders.Add(obj).Entity;
        }

        public Order GetById(string id)
        {
            return Context.Orders
                .SingleOrDefault(order => order.Id == id);
        }
        
        public Order Update(Order obj)
        {
            return Context.Orders.Update(obj).Entity;
        }

        public void Remove(Order obj)
        {
            Context.Orders.Remove(obj);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}