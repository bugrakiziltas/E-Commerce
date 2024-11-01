using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
        public class Order
        {
            [Key]
            public int Id {get;set;}
            public List<int> productIds {get;set;}
            public string userId {get;set;}
            public string email {get;set;}
            public string username {get;set;}
        }
}