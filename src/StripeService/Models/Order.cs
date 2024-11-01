using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeService.Models
{
    public class Order
    {
        public string userId {get;set;}
        public string email {get;set;}
        public string username {get;set;}
        public List<int> productIds {get;set;}
    }
}