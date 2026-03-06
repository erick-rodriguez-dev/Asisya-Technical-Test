using asisya.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace asisya.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Shipper> Shippers => Set<Shipper>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.CategoryID);
            e.HasIndex(c => c.CategoryName).IsUnique();
            e.Property(c => c.CategoryName).HasMaxLength(15).IsRequired();
            e.Property(c => c.Description).HasColumnType("text");
        });

        modelBuilder.Entity<Supplier>(e =>
        {
            e.HasKey(s => s.SupplierID);
            e.HasIndex(s => s.CompanyName);
            e.Property(s => s.CompanyName).HasMaxLength(40).IsRequired();
            e.Property(s => s.ContactName).HasMaxLength(30);
            e.Property(s => s.ContactTitle).HasMaxLength(30);
            e.Property(s => s.Address).HasMaxLength(60);
            e.Property(s => s.City).HasMaxLength(15);
            e.Property(s => s.Region).HasMaxLength(15);
            e.Property(s => s.PostalCode).HasMaxLength(10);
            e.Property(s => s.Country).HasMaxLength(15);
            e.Property(s => s.Phone).HasMaxLength(24);
            e.Property(s => s.Fax).HasMaxLength(24);
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.ProductID);
            e.HasIndex(p => p.ProductName);
            e.Property(p => p.ProductName).HasMaxLength(40).IsRequired();
            e.Property(p => p.QuantityPerUnit).HasMaxLength(20);
            e.Property(p => p.UnitPrice).HasColumnType("numeric(10,2)").HasDefaultValue(0m);
            e.Property(p => p.UnitsInStock).HasDefaultValue((short)0);
            e.Property(p => p.UnitsOnOrder).HasDefaultValue((short)0);
            e.Property(p => p.ReorderLevel).HasDefaultValue((short)0);
            e.Property(p => p.Discontinued).HasDefaultValue(false);

            e.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierID)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(c => c.CustomerID);
            e.Property(c => c.CustomerID).HasMaxLength(5);
            e.HasIndex(c => c.CompanyName);
            e.Property(c => c.CompanyName).HasMaxLength(40).IsRequired();
            e.Property(c => c.ContactName).HasMaxLength(30);
            e.Property(c => c.ContactTitle).HasMaxLength(30);
            e.Property(c => c.Address).HasMaxLength(60);
            e.Property(c => c.City).HasMaxLength(15);
            e.Property(c => c.Region).HasMaxLength(15);
            e.Property(c => c.PostalCode).HasMaxLength(10);
            e.Property(c => c.Country).HasMaxLength(15);
            e.Property(c => c.Phone).HasMaxLength(24);
            e.Property(c => c.Fax).HasMaxLength(24);
        });

        modelBuilder.Entity<Employee>(e =>
        {
            e.HasKey(emp => emp.EmployeeID);
            e.HasIndex(emp => new { emp.LastName, emp.FirstName });
            e.HasIndex(emp => new { emp.PostalCode });
            e.Property(emp => emp.LastName).HasMaxLength(20).IsRequired();
            e.Property(emp => emp.FirstName).HasMaxLength(10).IsRequired();
            e.Property(emp => emp.Title).HasMaxLength(30);
            e.Property(emp => emp.TitleOfCourtesy).HasMaxLength(25);
            e.Property(emp => emp.Address).HasMaxLength(60);
            e.Property(emp => emp.City).HasMaxLength(15);
            e.Property(emp => emp.Region).HasMaxLength(15);
            e.Property(emp => emp.PostalCode).HasMaxLength(10);
            e.Property(emp => emp.Country).HasMaxLength(15);
            e.Property(emp => emp.HomePhone).HasMaxLength(24);
            e.Property(emp => emp.Extension).HasMaxLength(4);
            e.Property(emp => emp.Notes).HasColumnType("text");

            e.HasOne(emp => emp.Manager)
                .WithMany(emp => emp.Subordinates)
                .HasForeignKey(emp => emp.ReportsTo)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Shipper>(e =>
        {
            e.HasKey(s => s.ShipperID);
            e.Property(s => s.CompanyName).HasMaxLength(40).IsRequired();
            e.Property(s => s.Phone).HasMaxLength(24);
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.OrderID);
            e.HasIndex(o => o.CustomerID);
            e.HasIndex(o => o.EmployeeID);
            e.HasIndex(o => o.OrderDate);
            e.HasIndex(o => o.ShippedDate);
            e.HasIndex(o => o.ShipPostalCode);
            e.Property(o => o.Freight).HasColumnType("numeric(10,2)").HasDefaultValue(0m);
            e.Property(o => o.ShipName).HasMaxLength(40);
            e.Property(o => o.ShipAddress).HasMaxLength(60);
            e.Property(o => o.ShipCity).HasMaxLength(15);
            e.Property(o => o.ShipRegion).HasMaxLength(15);
            e.Property(o => o.ShipPostalCode).HasMaxLength(10);
            e.Property(o => o.ShipCountry).HasMaxLength(15);

            e.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.Employee)
                .WithMany(emp => emp.Orders)
                .HasForeignKey(o => o.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.Shipper)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ShipVia)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderDetail>(e =>
        {
            e.HasKey(od => new { od.OrderID, od.ProductID });
            e.Property(od => od.UnitPrice).HasColumnType("numeric(10,2)").HasDefaultValue(0m);
            e.Property(od => od.Quantity).HasDefaultValue((short)1);
            e.Property(od => od.Discount).HasDefaultValue(0f);

            e.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
