using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            var orderRequest=new OrderRequest{
                userId=context.Message.userId,
                email=context.Message.email,
                username=context.Message.username,
                PaymentIntentId=context.Message.PaymentIntentId,
                productRequests=context.Message.products.Select(x=>new ProductRequest{
                    Id=x.Id,
                    Name=x.Name,
                    Price=x.Price,
                    ImageUrl=x.ImageUrl,
                }).ToList()
            };
            await _orderRepository.AddOrder(orderRequest);
        }
    }
}