using ContosoServices.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            ICategoryService service
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
                return NotFound("No se encontró el product category.");
            }

            return Ok(found);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public ActionResult Post([FromBody] ProductCategory myNewProductCategory)
        {
            _logger.LogInformation("Llamado del INSERT ejecutandose");

            var category = _service.Insert(myNewProductCategory);

            if (category == null)
            {
                return StatusCode(500, "Product category erróneo.");
            }
            else
            {
                return CreatedAtAction(nameof(Get), category);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProductCategory myProductCategory)
        {
            _logger.LogInformation("Llamado del UPDATE ejecutandose");

            (int status, string result) = _service.Update(id, myProductCategory);

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
