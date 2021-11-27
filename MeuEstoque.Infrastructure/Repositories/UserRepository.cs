using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;
using MeuEstoque.Infrastructure.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace MeuEstoque.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository, IUserPasswordStore<User>
    {
        private ApplicationContext Context { get; }

        private IEncypter Encrypter { get; }

        public IQueryable<User> All => Context.Users;

        public UserRepository(ApplicationContext context, IEncypter encypter)
        {
            Context = context;
            Encrypter = encypter;
        }

        public User Add(User obj)
        {
            var updatedUser = Context.Users.Add(obj).Entity;
            Context.SaveChanges();

            return updatedUser;
        }

        public User GetById(string id)
        {
            return Context.Users
                .SingleOrDefault(user => user.Id == id);
        }

        public User Update(User obj)
        {
            var updatedUser = Context.Users.Update(obj).Entity;
            Context.SaveChanges();

            return updatedUser;
        }

        public void Remove(User obj)
        {
            Context.Users.Remove(obj);
            Context.SaveChanges();
        }

        public IQueryable<User> FindByUsername(string username)
        {
            return Context.Users
                .Where(user => user.Username == username);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;
            return Context.SaveChangesAsync(cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.Name = normalizedName;
            return Context.SaveChangesAsync(cancellationToken);
        }
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(Encrypter.Encrypt(user.Password));
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        async Task<IdentityResult> IUserStore<User>.CreateAsync(User user, CancellationToken cancellationToken)
        {
            Context.Users.Add(user);
            await Context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            Context.Users.Update(user);
            await Context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            Context.Users.Remove(user);
            await Context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await Context.Users.FindAsync(new object[] {userId}, cancellationToken);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}