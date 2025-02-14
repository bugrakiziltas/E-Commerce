using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models.RequestModel;

namespace OrderService.Models
{
        public class Order
        {
            [Key]
            public int Id {get;set;}
            public string userId {get;set;}
            public string email {get;set;}
            public string username {get;set;}
            public string PaymentIntentId {get;set;}
            public DateTime orderDate {get;set;}=DateTime.UtcNow;
            public List<OrderBuyedProduct> orderBuyedProducts {get;set;}
        }
}