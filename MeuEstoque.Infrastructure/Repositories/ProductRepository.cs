using System.Linq;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;

namespace MeuEstoque.Infrastructure.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        ApplicationContext Context { get; }

        public IQueryable<Product> All => Context.Products;

        public ProductRepository(ApplicationContext context)
        {
            Context = context;
        }

        public Product Add(Product obj)
        {
            return Context.Products.Add(obj).Entity;
        }

        public Product GetById(string id)
        {
            return Context.Products
                .SingleOrDefault(Product => Product.Id == id);
        }

        public Product Update(Product obj)
        {
            return Context.Products.Update(obj).Entity;
        }


        public void Remove(Product obj)
        {
            Context.Products.Remove(obj);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}