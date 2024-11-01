using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Models.Entities;

namespace ShoppingCartService.Data
{
    public class ShoppingCartDbContext:DbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options):base(options){}
        public DbSet<ShoppingCartItem> ShoppingCartItems {get;set;}
        public DbSet<Product> Products {get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ShoppingCartItem>().Property(x=>x.Id).UseIdentityAlwaysColumn();
            modelBuilder.Entity<ShoppingCartItem>().Property(x=>x.Id).HasIdentityOptions(startValue:1);
        }
    }
}