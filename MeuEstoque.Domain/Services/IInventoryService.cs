using MeuEstoque.Domain.AggregatesModel.OrderAggregate;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;

namespace MeuEstoque.Domain.Services;

public interface IInventoryService
{
    void AddProduct(Product product);

    void AddOrder(Order order);
}