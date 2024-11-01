using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Models.Entities;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;

namespace ProductService.Services.IRepositories
{
    public interface IProductRepository
    {
        Task<Product> UpdateProduct(UpdateProductRequest requestModel);
        Task<CreateProductResponse> CreateProduct(CreateProductRequest requestModel);
        Task DeleteProduct(Product product);
        Task<Product> GetProductById(int id);
        Task<List<string>> GetProductsByIds(List<int> ids);
    }
}