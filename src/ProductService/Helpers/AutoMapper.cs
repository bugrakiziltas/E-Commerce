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
            CreateMap<Models.Entities.Product,ProductCreated>().ReverseMap();
            CreateMap<Models.Entities.Product,ProductUpdated>().ReverseMap();
            CreateMap<Models.Entities.Product,ProductDeleted>().ReverseMap();
            CreateMap<CreateProductResponse,Models.Entities.Product>().ReverseMap();
        }
    }
}