using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class BuyedProduct
    {
        [Key]
        public int Id {get;set;}
        public string Name {get;set;}
        public string ImageUrl {get;set;}
        public decimal Price {get;set;}
        public List<OrderBuyedProduct> orderBuyedProducts {get;set;}

    }
}