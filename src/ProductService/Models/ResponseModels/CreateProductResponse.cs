using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Models.ResponseModels
{
    public class CreateProductResponse
    {
        public string Name {get;set;}
        public decimal Price {get;set;}
        public string ImageUrl {get;set;}
    }
}