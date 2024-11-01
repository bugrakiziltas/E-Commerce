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
            var order=new Order{
                productIds=request.productIds,
                userId=request.userId,
                email=request.email,
                username=request.username
            };
            await _context.Orders.AddAsync(order);
            if(await _context.SaveChangesAsync()>0)
            {
                return order;
            }
            return null;
        }

        public async Task<List<int>> GetProductIdsOwnedByCustomer(string id)
        {
            var productIds=new List<int>();
            var ordersOfUser=await _context.Orders.Where(x=>x.userId==id).ToListAsync();
            foreach(var order in ordersOfUser)
            {
                productIds.AddRange(order.productIds);
            }
            return productIds;
        }
    }   
}