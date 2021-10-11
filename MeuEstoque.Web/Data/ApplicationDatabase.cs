using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MeuEstoque.Models;
using System.Linq;

namespace MeuEstoque.Data
{
    public class ApplicationDatabase : DbContext, IUserPasswordStore<User>
    {
        public IEncypter Encrypter { get; }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public ApplicationDatabase(DbContextOptions<ApplicationDatabase> options, IEncypter encrypter)
          : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }

            Encrypter = encrypter;
        }

        public override int SaveChanges()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.Entity is DBItem item)
                if (entry.State == EntityState.Modified)
                {
                    item.UpdatedAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DBItem>()
                .HasKey(item => item.Id);

            builder.Entity<DBItem>()
                .Property(item => item.CreatedAt)
                .IsRequired();

            builder.Entity<DBItem>()
                .Property(item => item.UpdatedAt)
                .IsRequired();

            builder.Entity<User>()
                .Property(user => user.Username)
                .IsRequired();

            builder.Entity<User>()
                .Property(user => user.Email)
                .IsRequired();

            builder.Entity<User>()
                .Property(user => user.Password)
                .HasConversion(
                    pwd => Encrypter.Encrypt(pwd),
                    pwd => Encrypter.Decrypt(pwd)
                )
                .IsRequired();

            builder.Entity<User>()
                .HasIndex(user => user.Username)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            builder.Entity<Product>()
                .HasOne(product => product.Owner)
                .WithMany(owner  => owner.Products)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(order => order.Owner)
                .WithMany(owner  => owner.Orders)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(order => order.Product)
                .WithMany(product  => product.Orders)
                .OnDelete(DeleteBehavior.Cascade);
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
            return SaveChangesAsync(cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.Name = normalizedName;
            return SaveChangesAsync(cancellationToken);
        }
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        async Task<IdentityResult> IUserStore<User>.CreateAsync(User user, CancellationToken cancellationToken)
        {
            Users.Add(user);
            await SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            Users.Update(user);
            await SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            Users.Remove(user);
            await SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await Users.FindAsync(new object[] {userId}, cancellationToken);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
