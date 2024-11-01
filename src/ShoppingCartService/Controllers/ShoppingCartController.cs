using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Models.Entities;
using ShoppingCartService.Models.RequestModels;
using ShoppingCartService.Service.IRepositories;

namespace ShoppingCartService.Controllers
{
    [Authorize(Policy ="Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IConfiguration _configuration;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IConfiguration configuration)
        {
            _shoppingCartRepository=shoppingCartRepository;
            _configuration=configuration;
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddToShoppingCart([FromBody]Product product)
        {
            var userId=User.FindFirst("id")?.Value;
            ShoppingCartItem shoppingCartItem=new ShoppingCartItem{
                UserId=userId,
                Product=product
            };
            var result=await _shoppingCartRepository.AddToShoppingCart(shoppingCartItem);
            if(result==null){
                return BadRequest("The product already added to shopping cart");
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetShoppingCartItems()
        {
            var userId=User.FindFirst("id")?.Value;
            var items=_shoppingCartRepository.GetShoppingCartProducts(userId);
            return Ok(items);
        }
        [HttpPost]
        [Route("complete")]
        public async Task<IActionResult> CompleteOrder()
        {
            var userId=User.FindFirst("id")?.Value;
            var productlist=await _shoppingCartRepository.GetShoppingCartProducts(userId);
            var orderRequest=new orderRequest{
                products=productlist,
                userId=userId,
                email=User.FindFirst(_configuration["Email"])?.Value,
                username=User.FindFirst("username")?.Value
            };
            var stripeResponse=await _shoppingCartRepository.CompleteOrder(orderRequest);
            return Ok(stripeResponse);
        }
    }
}