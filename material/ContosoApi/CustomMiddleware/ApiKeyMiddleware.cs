using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ContosoApi
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private string _API_KEY;

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _API_KEY = configuration.GetSection("API_KEY").Value;

            logger.LogInformation("Middleware personalizado");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("Middleware en ejecucion");

            if (!httpContext.Request.Path.Value.Contains("swagger"))
            {
                if (httpContext.Request.Headers.TryGetValue("x-api-key", out var key) is false)
                {
                    _logger.LogInformation("API KEY NOT FOUND");
                    httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    string responseMessage = JsonConvert.SerializeObject(new { Message = "API KEY Invalid." });
                    await httpContext.Response.WriteAsync(responseMessage);
                }

                if (string.IsNullOrEmpty(key))
                {
                    _logger.LogInformation("API KEY NOT FOUND");
                    httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    string responseMessage = JsonConvert.SerializeObject(new { Message = "API KEY Invalid." });
                    await httpContext.Response.WriteAsync(responseMessage);
                }

                if (!key.Equals(_API_KEY))
                {
                    _logger.LogInformation("API KEY INVALID.");
                    httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    string responseMessage = JsonConvert.SerializeObject(new { Message = "API KEY Invalid." });
                    await httpContext.Response.WriteAsync(responseMessage);
                }
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiKeyMiddlewareExtensions
    {
        //Extension Method
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
