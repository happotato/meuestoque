using System.Linq;
using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> FindByUsername(string username);
    }
}