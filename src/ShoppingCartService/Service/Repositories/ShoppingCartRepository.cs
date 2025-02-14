using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Data;
using ShoppingCartService.Models.Entities;
using ShoppingCartService.Models.ReponseModels;
using ShoppingCartService.Models.RequestModels;
using ShoppingCartService.Service.IRepositories;

namespace ShoppingCartService.Service.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ShoppingCartDbContext _context;
        private readonly IConfiguration _configuration;
        public ShoppingCartRepository(ShoppingCartDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context=context;
            _httpClient=httpClient;
            _configuration=configuration;
        }
        public async Task<ShoppingCartItem> AddToShoppingCart(ShoppingCartItem item)
        {
            var createdShoppingCartItem=await _context.Products.FirstOrDefaultAsync(x=>x.Id==item.Product.Id);
            if(createdShoppingCartItem!=null){
                return null;
            }
            _context.ShoppingCartItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<Product>> GetShoppingCartProducts(string userId)
        {
            return await _context.ShoppingCartItems.Include("Product").Where(x=>x.UserId==userId).Select(x=>x.Product).ToListAsync();
        }
        public async Task<ShoppingCartItem> DeleteFromShoppingCart(int id)
        {
            var shoppingCartItem=await _context.ShoppingCartItems.FirstOrDefaultAsync(x=>x.Id==id);
            if(shoppingCartItem==null) return null;
            _context.ShoppingCartItems.Remove(shoppingCartItem);
            if(await _context.SaveChangesAsync()>0) return shoppingCartItem;
            return null;
        }
        public async Task<StripeResponse> CompleteOrder(orderRequest request)
        {
            var response=await _httpClient.PostAsJsonAsync($"{_configuration["StripeUrl"]}/api/Checkout",request);
            if(response.IsSuccessStatusCode)
            {
                var stripeResponse=await response.Content.ReadFromJsonAsync<StripeResponse>();
                if(stripeResponse!=null)
                {
                    return stripeResponse;
                }
                return null;
            }
            return null;
        }
    }
}