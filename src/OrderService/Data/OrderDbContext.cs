using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }
        public DbSet<Order> Orders {get;set;}
        public DbSet<BuyedProduct> BuyedProducts {get;set;}
        public DbSet<OrderBuyedProduct> OrderBuyedProducts {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderBuyedProduct>()
            .HasKey(ob => new { ob.orderId, ob.buyedProductId });
            modelBuilder.Entity<Order>().Property(x=>x.Id).UseIdentityAlwaysColumn();
            modelBuilder.Entity<Order>().Property(x=>x.Id).HasIdentityOptions(startValue:1);
            modelBuilder.Entity<Order>().Property(x=>x.Id).UseIdentityAlwaysColumn();
            modelBuilder.Entity<OrderBuyedProduct>()
            .HasOne(ob => ob.order)
            .WithMany(o => o.orderBuyedProducts)
            .HasForeignKey(ob => ob.orderId);
            modelBuilder.Entity<OrderBuyedProduct>()
            .HasOne(ob => ob.buyedProduct)
            .WithMany(bp => bp.orderBuyedProducts)
            .HasForeignKey(ob => ob.buyedProductId);
        }
    }
}