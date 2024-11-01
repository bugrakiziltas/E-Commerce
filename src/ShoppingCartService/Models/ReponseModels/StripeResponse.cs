using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartService.Models.ReponseModels
{
    public class StripeResponse
    {
        public string? sessionId {get;set;}
        public string? checkoutUrl {get;set;}
    }
}