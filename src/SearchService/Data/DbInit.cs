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
            var products=JsonSerializer.Deserialize<List<Product>>(productData,options);
            await DB.SaveAsync(products);
        }
    }
}