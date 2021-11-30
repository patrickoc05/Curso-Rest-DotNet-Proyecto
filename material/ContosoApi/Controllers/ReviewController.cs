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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(
            ILogger<ReviewController> logger,
            IReviewService service
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
        public ActionResult Post([FromBody] ProductReview myNewProductReview)
        {
            _logger.LogInformation("Llamado del INSERT ejecutandose");

            var review = _service.Insert(myNewProductReview);

            if (review == null)
            {
                return StatusCode(500, "Product review erróneo.");
            }
            else
            {
                return CreatedAtAction(nameof(Get), review);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProductReview myProductReview)
        {
            _logger.LogInformation("Llamado del UPDATE ejecutandose");

            (int status, string result) = _service.Update(id, myProductReview);

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
