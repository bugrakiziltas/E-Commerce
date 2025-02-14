using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Contracts
{
    public class OrderCreated
    {
        public List<Product> products {get;set;}
        public string userId {get;set;}
        public string email {get;set;}
        public string username {get;set;}
        public string PaymentIntentId {get;set;}
    }
    public class Product{
        public int Id {get;set;}
        public string Name {get;set;}
        public string ImageUrl {get;set;}
        public decimal Price {get;set;}
    }
}