using System;
using Microsoft.EntityFrameworkCore;
using MeuEstoque.Domain.SharedKernel;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;
using MeuEstoque.Domain.AggregatesModel.ProductAggregate;
using MeuEstoque.Domain.AggregatesModel.OrderAggregate;
using MeuEstoque.Infrastructure.Cryptography;

namespace MeuEstoque.Infrastructure;

public sealed class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; private set; }

    public DbSet<Product> Products { get; private set; }

    public DbSet<Order> Orders { get; private set; }

    public IEncypter Encrypter { get; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options, IEncypter encrypter)
        : base(options)
    {
        Encrypter = encrypter;

        if (Database.IsRelational())
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Entity>()
            .HasKey(item => item.Id);

        builder.Entity<Entity>()
            .Property(item => item.CreatedAt)
            .IsRequired();

        builder.Entity<Entity>()
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

    public override int SaveChanges()
    {
        foreach (var entry in this.ChangeTracker.Entries())
        {
            if (entry.Entity is Entity entity)
            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }
}
