using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Domain.AggregatesModel.UserAggregate
{
    public sealed class User : Entity, IAggregateRoot
    {
        public string Name { get; }
        public string Username { get; }
        public string Email { get; }
        public string Password { get; }

        public User(string name, string username, string email, string password)
        {
            Name = name;
            Username = username;
            Email = email;
            Password = password;
        }
    }
}