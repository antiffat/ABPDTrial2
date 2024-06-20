using APBDTrial_2.Models;
using Microsoft.EntityFrameworkCore;

namespace APBDTrial_2.Helpers;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationships
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade); // If a customer is deleted, all related orders should also be deleted.

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // If an order is deleted, all related order items should also be deleted.

        modelBuilder.Entity<Product>()
            .HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // A product should not be deleted if there are existing order items referencing it.

        modelBuilder.Entity<Supplier>()
            .HasMany(s => s.Products)
            .WithOne(p => p.Supplier)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict); // A supplier should not be deleted if there are existing products referencing it.

        // Configure properties and indexes
        modelBuilder.Entity<Customer>()
            .Property(c => c.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Phone)
            .IsUnique();

        modelBuilder.Entity<Order>()
            .Property(o => o.OrderDate)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Order>()
            .Property(o => o.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Quantity)
            .IsRequired();

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Product>()
            .Property(p => p.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.Package)
            .HasMaxLength(50);

        modelBuilder.Entity<Product>()
            .Property(p => p.IsDiscounted)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(p => p.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.ProductName)
            .IsUnique();

        modelBuilder.Entity<Supplier>()
            .Property(s => s.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.CompanyName)
            .IsUnique();

        // Seed data
        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, CompanyName = "Supplier1", ContactName = "Contact1", City = "City1", Country = "Country1", Phone = "1234567890", Fax = "0987654321" },
            new Supplier { Id = 2, CompanyName = "Supplier2", ContactName = "Contact2", City = "City2", Country = "Country2", Phone = "2234567890", Fax = "1987654321" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, ProductName = "Product1", SupplierId = 1, UnitPrice = 10, Package = "Package1", IsDiscounted = false },
            new Product { Id = 2, ProductName = "Product2", SupplierId = 2, UnitPrice = 20, Package = "Package2", IsDiscounted = true }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FirstName = "John", LastName = "Doe", City = "City1", Country = "Country1", Phone = "1234567890" },
            new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", City = "City2", Country = "Country2", Phone = "2234567890" }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, CustomerId = 1, OrderDate = new DateTime(2023, 1, 1), TotalAmount = 100 },
            new Order { Id = 2, CustomerId = 2, OrderDate = new DateTime(2023, 1, 2), TotalAmount = 200 }
        );

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, ProductId = 1, UnitPrice = 10, Quantity = 10 },
            new OrderItem { Id = 2, OrderId = 2, ProductId = 2, UnitPrice = 20, Quantity = 10 }
        );
    }
}