using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.Helpers
{
    public class Automapper:Profile
    {
        public Automapper()
        {
            CreateMap<ProductCreated,SearchService.Models.Product>().ReverseMap();
            CreateMap<ProductUpdated,SearchService.Models.Product>().ReverseMap();
            CreateMap<ProductDeleted,SearchService.Models.Product>().ReverseMap();
        }
    }
}