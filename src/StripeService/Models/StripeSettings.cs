using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeService.Models
{
    public class StripeSettings
    {
        public string? SecretKey {get;set;}
        public string? WHSecret {get;set;}
    }
}