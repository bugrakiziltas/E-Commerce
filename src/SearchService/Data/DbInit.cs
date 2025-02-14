using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data
{
    public class DbInit
    {
        public static async Task InitDb(WebApplication app){
            await DB.InitAsync("SearchDB",MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("ConnectionString")));

            await DB.Index<Product>().Key(x=>x.Name,KeyType.Text).CreateAsync();

            var count=await DB.CountAsync<Product>();
            if(count>0){
                return;
            }
            var productData= await File.ReadAllTextAsync("Data/products.json");
            var options=new JsonSerializerOptions{
                PropertyNameCaseInsensitive=true
            };
            var productsWithIds=JsonSerializer.Deserialize<List<ProductWithId>>(productData,options);
            var products=new List<Product>();
            foreach(var productWithId in productsWithIds)
            {
                products.Add(new Product{
                    ID=productWithId.ProductId.ToString(),
                    Name=productWithId.Name,
                    Price=productWithId.Price,
                    ImageUrl=productWithId.ImageUrl
                });
            }
            await DB.SaveAsync(products);
        }
    }
    public class ProductWithId{
        public int ProductId {get;set;}
        public string Name {get;set;}
        public decimal Price {get;set;}
        public string ImageUrl {get;set;}
    }
}