using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContosoServices.Services;

namespace ProductsApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            ILogger<ProductsController> logger,
            IProductService service
        )
        {
            _service = service;
            _logger = logger;

            _logger.LogInformation("DI correcto");
        }

        //[HttpGet]
        //public ActionResult Get()
        //{
        //    _logger.LogInformation("Llamado del GET ejecutandose");

        //    return Ok(_service.Get());
        //}

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            _logger.LogInformation("Llamado del GET ejecutandose");

            var found = _service.Get(id);

            if (found == null)
            {
                return NotFound("No se encontró el producto.");
            }

            return Ok(found);
        }

        //int id, string name, int categoryId, string propertyOrder, string typeOrder, int limit
        [HttpGet]
        public ActionResult Get([FromQuery] int id, [FromQuery] string name, [FromQuery] int categoryId, [FromQuery] string propertyOrder, [FromQuery] string typeOrder, [FromQuery] int limit)
        {
            _logger.LogInformation("Llamado del GET ejecutandose");

            if(id == 0 && name == null && categoryId == 0 && propertyOrder == null && typeOrder == null && limit == 0)
            {
                return Ok(_service.Get());
            }

            return Ok(_service.Get(id, name, categoryId, propertyOrder, typeOrder, limit));
        }

        // POST api/<ProductsController>
        [HttpPost]
        public ActionResult Post([FromBody] Product myNewProduct)
        {
            _logger.LogInformation("Llamado del INSERT ejecutandose");

            var product = _service.Insert(myNewProduct);

            if (product == null)
            {
                return StatusCode(500, "Producto erroneo.");
            }
            else
            {
                return CreatedAtAction(nameof(Get), product);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Product myNewProduct)
        {
            _logger.LogInformation("Llamado del UPDATE ejecutandose");

            (int status, string result) = _service.Update(id, myNewProduct);

            if (result == null)
            {
                return StatusCode(status, "Error leyendo el resultado de la implementación.");
            }
            else
            {
                if (status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(status, result);
                }
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("Llamado del DELETE ejecutandose");

            (int status, string result) = _service.Delete(id);

            if (result == null)
            {
                return StatusCode(status, "Error leyendo el resultado de la implementación.");
            }
            else
            {
                if (status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(status, result);
                }
            }
        }
    }
}
