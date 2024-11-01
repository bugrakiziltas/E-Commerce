using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models.Entities;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;
using ProductService.Services.IRepositories;

namespace ProductService.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public ProductRepository(ProductDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint){
            _context=context;
            this.mapper=mapper;
            _publishEndpoint=publishEndpoint;
        }
        public async Task<CreateProductResponse> CreateProduct(CreateProductRequest requestModel)
        {
        using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        var product = new Product
        {
            Name = requestModel.Name,
            Price = requestModel.Price,
            ImageUrl = requestModel.ImageUrl
        };
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync(); 
        
       await _publishEndpoint.Publish(mapper.Map<ProductCreated>(product));
        
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        var productDTO = mapper.Map<CreateProductResponse>(product);
        return productDTO;
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            await _publishEndpoint.Publish(mapper.Map<ProductDeleted>(product));
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            var product=await _context.Products.FirstOrDefaultAsync(x=>x.Id==id);
            if(product==null){
                return null;
            }
            return product;
        }

        public async Task<List<string>> GetProductsByIds(List<int> ids)
        {
            return await _context.Products.Where(x=>ids.Contains(x.Id)).Select(x=>x.Name).ToListAsync();
        }

        public async Task<Product> UpdateProduct(UpdateProductRequest requestModel)
        {
            var product=await _context.Products.FirstOrDefaultAsync(x=>x.Id==requestModel.Id);
            if(product==null){
                return null;
            }
            product.Name=requestModel.Name;
            product.ImageUrl=requestModel.ImageUrl;
            product.Price=requestModel.Price;
            await _publishEndpoint.Publish(mapper.Map<ProductUpdated>(product));
            if(await _context.SaveChangesAsync()> 0)
            {
                return product;
            }
            return null;
        }
    }
}