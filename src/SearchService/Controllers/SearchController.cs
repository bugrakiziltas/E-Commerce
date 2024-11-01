using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Helpers;
using SearchService.Models;
using ZstdSharp.Unsafe;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public SearchController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient=httpClient;
            _configuration=configuration;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> SearchProducts([FromQuery]SearchParams searchParams)
        {
            var query= DB.PagedSearch<Product,Product>();
            // example: Red dead redemption 2 contains red.    
            if(!String.IsNullOrEmpty(searchParams.searchContains)){
                 query.Match(Search.Full, searchParams.searchContains).SortByTextScore();
            }
            // ordering the products according to upcoming parameter
            query=searchParams.orderBy switch{
                "name"=> query.Sort(x=>x.Ascending(x=>x.Name)),
                "price" =>query.Sort(x=>x.Ascending(x=>x.Price)),
                 _ =>query.Sort(x=>x.Ascending(x=>x.Name))
            };
            // filtering the products according to upcoming parameter. Will be changed when the product has more properties.
            // like if the product has discount it will be filtered.
            query=searchParams.FilterBy switch{
                _ =>query.Match(x=>x.Price>0)
            };
            query.PageNumber(searchParams.pageNumber);
            query.PageSize(searchParams.pageSize);
            var userId=User.FindFirst("id")?.Value;
            if(userId!=null){
                var ownedProductNames=await GetProductNames(userId);
                query.Match(x=>!ownedProductNames.Contains(x.Name));
            }
            var result=await query.ExecuteAsync();
            List<Product> products=result.Results.ToList();
            return products;
        }
        [HttpGet]
        [Route("getbyname")]
        public async Task<IActionResult> GetProductByName([FromQuery]string name)
        {
            var product=await DB.Find<Product>().Match(x=>x.Name==name).ExecuteAsync();
            if(product==null){
                return BadRequest("The product does not exists");
            }
            return Ok(product);
        }
        private async Task<List<int>> GetProductIds(string id)
        {
            var request=new GetProductIdsRequest{
                id=id
            };
            var response=await _httpClient.PostAsJsonAsync($"{_configuration["OrderServiceUrl"]}/api/Order/GetProducts",request);
            if(response.IsSuccessStatusCode)
            {
                var productIds=await response.Content.ReadFromJsonAsync<List<int>>();
                if(productIds!=null)
                {
                    return productIds;
                }
                return null;
            }
            return null;
        }
        private async Task<List<string>> GetProductNames(string userId)
        {
            var ids=await GetProductIds(userId);
            var request=new GetProductNamesOwnedByCustomerRequest{
                productIds=ids
            };
            var response=await _httpClient.PostAsJsonAsync($"{_configuration["ProductServiceUrl"]}/api/Product/GetProductNamesByIds",request);
            if(response.IsSuccessStatusCode)
            {
                var productNames=await response.Content.ReadFromJsonAsync<List<string>>();
                if(productNames!=null)
                {
                    return productNames;
                }
                return null;
            }
            return null;
        }
    }
}