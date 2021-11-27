using System.Linq;

namespace MeuEstoque.Domain.SharedKernel
{
    // Use AddScoped
    public interface IRepository<T> where T : Entity, IAggregateRoot
    {
        IQueryable<T> All { get; }

        T GetById(string id);

        T Add(T obj);

        T Update(T obj);

        void Remove(T obj);
    }
}