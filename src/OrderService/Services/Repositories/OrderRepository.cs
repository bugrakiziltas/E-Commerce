using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using OrderService.Models.RequestModel;
using OrderService.Services.IRepositories;

namespace OrderService.Services.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        public OrderRepository(OrderDbContext context)
        {
            _context=context;
        }
        
        public async Task<Order> AddOrder(OrderRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {

            var order = new Order
            {
                userId = request.userId,
                email = request.email,
                username = request.username,
                PaymentIntentId = request.PaymentIntentId
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync(); 


            var buyedProducts = request.productRequests.Select(product => new BuyedProduct
            {
                Id = product.Id, 
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            }).ToList();

            await _context.BuyedProducts.AddRangeAsync(buyedProducts);
            await _context.SaveChangesAsync();
            var orderBuyedProductList=new List<OrderBuyedProduct>();
            foreach(var product in buyedProducts){
                orderBuyedProductList.Add(new OrderBuyedProduct{
                    order=order,
                    buyedProduct=product,  
                });
            }
            await _context.OrderBuyedProducts.AddRangeAsync(orderBuyedProductList);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return order;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return null;
        }

}


        public async Task<List<int>> GetProductIdsOwnedByCustomer(string id)
        {
            var productIds=new List<int>();
            var ordersOfUser=await _context.Orders.Where(x=>x.userId==id).ToListAsync();
            foreach(var order in ordersOfUser)
            {
                productIds.AddRange(order.orderBuyedProducts.Select(x=>x.buyedProductId));
            }
            return productIds;
        }
    }   
}