using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class OrderBuyedProduct
    {
        public int orderId {get;set;}
        public Order order {get;set;}
        public int buyedProductId {get;set;}
        public BuyedProduct buyedProduct {get;set;}
        public bool isDownloaded {get;set;}=false;
    }
}