using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartService.Models.Entities;

namespace ShoppingCartService.Models.RequestModels
{
    public class orderRequest
    {
        public List<Product> products {get;set;}
        public string userId {get;set;}
        public string email {get;set;}
        public string username {get;set;}
    }
}