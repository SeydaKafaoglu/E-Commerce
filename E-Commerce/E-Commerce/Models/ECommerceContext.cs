
using Microsoft.EntityFrameworkCore;
using E_Commerce;
namespace E_Commerce.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class ECommerceContext : DbContext
{
    public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
    {


    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=seyda-pc;Initial Catalog=ECommerce; Integrated Security=True");

    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderDetailStatus> OrderDetailStatuses { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<ItemStatus> ItemStatuses { get; set; }




}
