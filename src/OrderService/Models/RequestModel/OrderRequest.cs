using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models.RequestModel
{
    public class OrderRequest
    {
        public List<int> productIds {get;set;}
        public string userId {get;set;}
        public string email {get;set;}
        public string username {get;set;}
    }
}