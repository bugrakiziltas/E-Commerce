using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models.RequestModel;
using OrderService.Services.IRepositories;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository=orderRepository;
        }
        [HttpPost]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProductsOwnedByCustomer([FromBody]CustomerRequest request)
        {
            var productIds=await orderRepository.GetProductIdsOwnedByCustomer(request.id);
            return Ok(productIds);
        }
    }
}