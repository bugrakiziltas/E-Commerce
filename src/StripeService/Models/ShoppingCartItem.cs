using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace StripeService.Models
{
    public class ShoppingCartItem
    {
        public int Id {get;set;}
        public string UserId {get;set;}
        public int ProductId {get;set;}
        [ForeignKey("ProductId")]
        [ValidateNever]
        public ProductInfo Product {get;set;}
    }
}