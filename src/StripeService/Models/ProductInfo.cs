using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeService.Models
{
    public class ProductInfo
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public decimal Price {get;set;}
        public string ImageUrl {get;set;}
    }
}