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
    public class ProductDeletedConsumer : IConsumer<ProductDeleted>
    {
        private readonly IMapper _mapper;
        public ProductDeletedConsumer(IMapper mapper)
        {
            _mapper=mapper;
        }

        public async Task Consume(ConsumeContext<ProductDeleted> context)
        {
            var product=_mapper.Map<Product>(context.Message);
            await product.DeleteAsync();
        }
    }
}