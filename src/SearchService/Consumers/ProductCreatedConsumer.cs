using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly IMapper _mapper;
        public ProductCreatedConsumer(IMapper mapper)
        {
            _mapper=mapper;
        }
        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var product=_mapper.Map<Product>(context.Message);
            await product.SaveAsync();
        }
    }
}