using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e_shop_api.Core.Config;
using e_shop_api.Core.Extensions;
using e_shop_api.Core.Utility;
using e_shop_api.Core.Utility.Interface;
using e_shop_api.Extensions;
using EasyNetQ;
using EShop.Cache.Interface;
using EShop.Cache.Utility;
using EShop.Entity.DataBase;
using EShop.Logic.Applications.Admin.Command.Login;
using EShop.Logic.Applications.Cart.Command.Update;
using EShop.Logic.Applications.Cart.Query;
using EShop.Logic.Applications.Product.Query;
using EShop.Logic.Applications.Products.Query;
using EShop.Logic.Applications.SystemCode.Query;
using EShop.Logic.Utility;
using EShop.MQ.Producer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StackExchange.Redis;

namespace e_shop_api
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "localhost";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // MediatR注入
            services.AddMediatR(typeof(LoginHandler).Assembly); // EShop.Logic
            services.AddScoped<QueryCartHandler>();
            services.AddScoped<CleanCartHandler>();
            services.AddScoped<QuerySystemCodeHandler>();

            // MQ
            services.AddScoped<MqProducer>();

            // 底層物件注入
            services.AddSingleton<IPageUtility, PageUtility>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // 固定使用memory cache
            services.AddSingleton<IBaseMemoryCacheUtility, BaseMemoryCacheUtility>();
            services.AddSingleton<IProductsCacheUtility, ProductsMemoryCacheUtility>();
            services.AddSingleton<ISystemCodeCacheUtility, SystemCodeMemoryCacheUtility>();
            services.AddSingleton<IAdminInfoCacheUtility, AdminInfoMemoryCacheUtility>();
            switch (Configuration["CacheType"])
            {
                case "memory":
                    services.AddSingleton<IShoppingCartCacheUtility, ShoppingCartMemoryCacheUtility>();
                    break;
                case "redis":
                    services.AddSingleton<IShoppingCartCacheUtility, ShoppingCartRedisCacheUtility>();
                    services.AddSingleton<IConnectionMultiplexer>(
                        ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnectionString")));
                    break;
            }

            // Jwt Start
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidIssuer = Configuration["JwtSettings:Issuer"],
                        ValidateAudience = false,
                        ValidAudience = Configuration["JwtSettings:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SignKey"]))
                    };
                });
            services.Configure<JwtConfig>(Configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtUtility, JwtUtility>();
            // Jwt End

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "e_shop_api", Version = "v1" });

                // swagger 支援 Jwt
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    { new OpenApiSecurityScheme() { }, new List<string>() }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            services.AddDbContext<EShopDbContext>(builder =>
            {
                builder.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"],
                    optionsBuilder =>
                    {
                        optionsBuilder.RemoteCertificateValidationCallback((sender, certificate, chain, errors) =>
                            true);
                    });
            });

            // CORS
            services.AddCors(
                options => options.AddPolicy(
                    DefaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            Configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // MQ
            var rabbitMqConnectionString = Configuration.GetValue<string>("RabbitMqTcpConnectionString");
            services.AddSingleton(typeof(IBus), RabbitHutch.CreateBus(rabbitMqConnectionString, x => { }));

            // ES
            services.AddElasticsearch(Configuration);
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

            // 自訂的Middleware
            app.UseCustomerExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseCors(DefaultCorsPolicyName); // Enable CORS!

            app.UseAuthentication();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            // MQ
            app.MessageQueueSubscribe();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}