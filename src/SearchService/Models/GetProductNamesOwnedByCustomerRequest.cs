using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchService.Models
{
    public class GetProductNamesOwnedByCustomerRequest
    {
        public List<int> productIds {get;set;}
    }
}