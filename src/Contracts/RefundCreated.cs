using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public class RefundCreated
    {
        public string RefundId {get;set;}
        public int ProductId {get;set;}
        public long Amount {get;set;}
        public string UserId {get;set;}
        
    }
}