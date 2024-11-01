using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Entities;

namespace SearchService.Models
{
    public class Product:Entity
    {
        public string Name {get;set;}
        public decimal Price {get;set;}
        public string ImageUrl {get;set;}
    }
}