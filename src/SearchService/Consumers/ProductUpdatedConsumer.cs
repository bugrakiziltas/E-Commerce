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
    public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
    {
        private readonly IMapper _mapper;
        public ProductUpdatedConsumer(IMapper mapper)
        {
            _mapper=mapper;
        }
        public async Task Consume(ConsumeContext<ProductUpdated> context)
        {
            var product=_mapper.Map<SearchService.Models.Product>(context.Message);
            await product.SaveAsync();
        }
    }
}