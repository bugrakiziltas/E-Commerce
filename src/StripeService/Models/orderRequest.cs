using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartService.Models;
using StripeService.Models;

namespace ShoppingCartService.Models.RequestModels
{
    public class orderRequest
    {
        public List<ProductInfo> products {get;set;}
        public string userId {get;set;}
        public string email {get;set;}
        public string username {get;set;}
    }
}