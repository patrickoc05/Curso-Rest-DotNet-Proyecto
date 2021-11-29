using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using ContosoServices.Services;

namespace ContosoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AdventureworksContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("AdventureWorks");

                Console.WriteLine(connectionString);
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<AdventureworksContext>();

            services.AddScoped<IProductService, ProductService>();
            //services.AddScoped<ICustomerService, CustomerService>();
            //services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<IOrderService, OrderService>();

            services.AddCors(options =>
            {
                options.AddPolicy("Policy",
                                builder =>
                                {
                                    builder.AllowAnyOrigin()
                                           .AllowAnyHeader()
                                           .AllowAnyMethod();
                                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContosoApi", Version = "v1" });
                c.AddSecurityDefinition("X-Api-Key", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "X-Api-Key",
                    Description = "Contoso Api Key",
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "X-Api-Key" }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "X-Api-Key",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-Api-Key"
                            },
                         },
                         new string[] {}
                     }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiKeyMiddleware();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContosoApi v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseRouting();
            app.UseCors("Policy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
