using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContosoServices.Services;

namespace ContosoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private IProductService _service;
        private ILogger<CustomersController> _logger;

        public CustomersController(
            ILogger<CustomersController> logger,
            IProductService service
        )
        {
            _service = service;
            _logger = logger;

            _logger.LogInformation("DI correcto");
        }

        [HttpGet]
        public ActionResult Get()
        {
            _logger.LogInformation("Llamado del GET ejecutandose");

            return Ok(_service.Get());
        }

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
                if (product.Equals("Producto insertado correctamente."))
                {
                    return CreatedAtAction(nameof(Get), product);
                }
                else
                {
                    return StatusCode(500, product);
                }
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
