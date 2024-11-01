using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using OrderService.Models;
using OrderService.Models.RequestModel;
using OrderService.Services.IRepositories;
using OrderService.Services.Repositories;

namespace OrderService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly IOrderRepository _orderRepository;
        public OrderCreatedConsumer(IOrderRepository orderRepository)
        {
            _orderRepository=orderRepository;
        }
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var order=new OrderRequest{
                productIds=context.Message.productIds,
                userId=context.Message.userId,
                email=context.Message.email,
                username=context.Message.username
            };
            var response=await _orderRepository.AddOrder(order);
        }
    }
}