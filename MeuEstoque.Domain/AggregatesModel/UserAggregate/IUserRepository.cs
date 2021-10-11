using System.Collections.Generic;
using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> FindByUsername(string id);
    }
}