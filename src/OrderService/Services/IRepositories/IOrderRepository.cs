using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models;
using OrderService.Models.RequestModel;

namespace OrderService.Services.IRepositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(OrderRequest request);
        Task<List<int>> GetProductIdsOwnedByCustomer(string id);
    }
}