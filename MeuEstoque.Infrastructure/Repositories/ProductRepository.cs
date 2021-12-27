using System.Linq;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;

namespace MeuEstoque.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private ApplicationContext Context { get; }

    public IQueryable<Product> All => Context.Products;

    public ProductRepository(ApplicationContext context)
    {
        Context = context;
    }

    public Product Add(Product obj)
    {
        var updatedProduct = Context.Products.Add(obj).Entity;
        Context.SaveChanges();

        return updatedProduct;
    }

    public Product GetById(string id)
    {
        return Context.Products
            .SingleOrDefault(Product => Product.Id == id);
    }

    public Product Update(Product obj)
    {
        var updatedProduct = Context.Products.Update(obj).Entity;
        Context.SaveChanges();

        return updatedProduct;
    }


    public void Remove(Product obj)
    {
        Context.Products.Remove(obj);
    }
}