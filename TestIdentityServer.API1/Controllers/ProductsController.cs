using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestIdentityServer.API1.Models;

namespace TestIdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>()
            {
                new Product { Id = 1, Name = "Kalem", Price = 100, Stock = 500 },
                new Product { Id = 2, Name = "Silgi", Price = 100, Stock = 500},
                new Product { Id = 3, Name = "Defter", Price = 100, Stock = 500},
            };

            return Ok(productList);
        }

        [Authorize(Policy = "UpdateOrCreate")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"{id} has been updated");
        }

        [Authorize(Policy = "UpdateOrCreate")]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product);
        }
    }
}
