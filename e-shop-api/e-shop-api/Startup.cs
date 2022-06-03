using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_shop_api.Applications.Product.Query;
using e_shop_api.DataBase;
using e_shop_api.Utility;
using e_shop_api.Utility.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace e_shop_api
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
            services.AddControllers();

            services.AddDbContext<EShopDbContext>(builder =>
            {
                builder.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"],
                    optionsBuilder =>
                    {
                        optionsBuilder.RemoteCertificateValidationCallback((sender, certificate, chain, errors) =>
                            true);
                    });
            });

            // MediatR注入
            services.AddMediatR(typeof(QueryProductHandler).Assembly);

            // 底層物件注入
            services.AddSingleton<IPageUtility, PageUtility>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "e_shop_api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "e_shop_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}