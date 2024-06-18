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

        modelBuilder.Entity<Order>()
            .Property(o => o.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Customer>()
            .Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50)
            .IsConcurrencyToken();
        
        modelBuilder.Entity<Customer>()
            
            .Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(50)
            .IsConcurrencyToken();
    }
}