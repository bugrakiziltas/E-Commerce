using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Models.Entities
{
    public class Product
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public decimal Price {get;set;}
        public string ImageUrl {get;set;}
    }
}