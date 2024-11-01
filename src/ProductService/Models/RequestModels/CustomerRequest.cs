using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Models.RequestModels
{
    public class CustomerRequest
    {
        public List<int> productIds {get;set;}
    }
}