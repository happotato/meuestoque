namespace MeuEstoque.Domain.SharedKernel
{
    public interface IRepository<T> where T : Entity, IAggregateRoot
    {
        void AddOrUpdate(T obj);
        void Remove(T obj);
        T GetById(T obj);
    }
}