using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.OrderAggregate
{
    public interface IProductRepository : IRepository<Order>
    {
        
    }
}