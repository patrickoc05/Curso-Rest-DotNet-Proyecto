using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private ServicesConfiguration Services { get; }


        public OrdersController(IConfiguration configuration)
        {
            Services = new ServicesConfiguration();

            configuration.GetSection("Services").Bind(Services);
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var httpClient = new HttpClient();// { BaseAddress = new Uri(Services.ProductsSvc) };

            //Order
            // -> Products [] -> Product, Quantity
            // -> Customers
            // -> Total
            var json = await httpClient.GetStringAsync("http://localhost:5002/api/v1/Products");
            var products = JsonConvert.DeserializeObject<JArray>(json);

            object[] lines = products.Select(line => (object)line).ToArray();

            object order1 = new { Lines = lines, Customer = new object(), Date = DateTime.Now, Total = (decimal)0 };

            return Ok(new object[] { order1, });
        }

        private class ServicesConfiguration
        {
            public string CustomersSvc { get; set; }
            public string ProductsSvc { get; set; }
        }
    }
}
