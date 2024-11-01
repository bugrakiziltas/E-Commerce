using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchService.Helpers
{
    public class SearchParams
    {
        public string? searchContains {get;set;}
        public int pageNumber {get;set;}=1;
        public int pageSize {get;set;}=3;
        public string? orderBy{get;set;}
        public string? FilterBy{get;set;}
    }
}