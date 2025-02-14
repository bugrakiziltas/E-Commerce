using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartService.Models.Entities;
using ShoppingCartService.Models.ReponseModels;
using ShoppingCartService.Models.RequestModels;

namespace ShoppingCartService.Service.IRepositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCartItem> AddToShoppingCart(ShoppingCartItem item);
        Task<List<Product>> GetShoppingCartProducts(string userId);
        Task<StripeResponse> CompleteOrder(orderRequest request);
        Task<ShoppingCartItem> DeleteFromShoppingCart(int id);
    }
}