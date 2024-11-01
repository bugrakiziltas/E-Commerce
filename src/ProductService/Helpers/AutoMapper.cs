using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using ProductService.Models.Entities;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;

namespace ProductService.Helpers
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<Product,ProductCreated>().ReverseMap();
            CreateMap<Product,ProductUpdated>().ReverseMap();
            CreateMap<Product,ProductDeleted>().ReverseMap();
            CreateMap<CreateProductResponse,Product>().ReverseMap();
        }
    }
}