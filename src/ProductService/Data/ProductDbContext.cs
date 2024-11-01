using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Models.Entities;
namespace ProductService.Data
{
    public class ProductDbContext:DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(x=>x.Id).UseIdentityAlwaysColumn();
            modelBuilder.Entity<Product>().Property(x=>x.Id).HasIdentityOptions(startValue:1);
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}