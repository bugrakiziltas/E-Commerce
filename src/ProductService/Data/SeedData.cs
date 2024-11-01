using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Models.Entities;

namespace ProductService.Data
{
    public class SeedData
    {
        public static async Task SeedProducts(ProductDbContext context){
            if(await context.Products.AnyAsync()){
               return ;
            }
            var productData=await File.ReadAllTextAsync("Data/products.json");
            var options=new JsonSerializerOptions{
                PropertyNameCaseInsensitive=true
            };
            var products=JsonSerializer.Deserialize<List<Product>>(productData,options);
            foreach(var product in products){
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
            }
        }
    }
}