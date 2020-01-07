using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildingBlocks.EventBusRabbitMQ;
using EventBus.Abstractions;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Application.IntegrationEvents.EventHandling;
using Ordering.API.Application.IntegrationEvents.Events;
using Ordering.API.Application.Queries;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }


        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //Identity
            services.AddAuthorization();
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options => 
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "orders";
                });


            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IBuyerRepository, BuyerRepository>();
            services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();
            services.AddScoped<IOrderQueries, OrderQueries>();
            var subscriptionClientName = Configuration["SubscriptionClientName"];

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                return new EventBusRabbitMQ(sp, subscriptionClientName);
            });

            services.AddDbContext<OrderingContext>(options => 
                {
                    options.UseSqlServer(Configuration.GetConnectionString("OrderingContext"));
                },ServiceLifetime.Scoped);


            services.AddDbContext<IntegrationEventLogContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("OrderingContext"),
                sqlOptions => sqlOptions.MigrationsAssembly("Ordering.Infrastructure"));
                }, ServiceLifetime.Scoped);


            RegisterEventBus(services);
            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>(sp =>
            {
                return new IntegrationEventLogService(Configuration.GetConnectionString("OrderingContext"));
            });

            
            services.AddMediatR(System.Reflection.Assembly.GetExecutingAssembly());
            //services.AddScoped(typeof(IFoo<,>), typeof(Foo<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(Application.Validation.ValidationBehavior<,>));


            //Install - Package Swashbuckle.AspNetCore - Version 5.0.0 - rc4
            //suppress warning 1591
            //https://exceptionnotfound.net/adding-swagger-to-asp-net-core-web-api-using-xml-documentation/

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering API", Version = "v1" });
                //var contentRoot = HostingEnvironment.ContentRootPath;
                //var path = $"{contentRoot}\\Ordering.API.xml";
                //c.IncludeXmlComments(path);
            });

            services.AddScoped<DoesBuyerExist, DoesBuyerExist>();
             services.AddCors(Options => { Options.AddPolicy("myPolicy",
                builder =>
                {
                    builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod().AllowCredentials();

                });
            });
        }
        private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];
            

            services.AddSingleton<IEventBus, EventBusRabbitMQ>( sp =>
            {
                
                return new EventBusRabbitMQ(sp,subscriptionClientName);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            loggerFactory.AddFile("Logs/orderingLogs.txt", LogLevel.Information, new Dictionary<string, LogLevel>()
            {
                { "Microsoft", LogLevel.Error },
                { "System", LogLevel.Error }
            });
            app.UseCors("myPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering API V1");
            });
            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();
            
            ConfigureEventBus(app);

        }
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<UserCheckoutIntegrationEvent, UserCheckoutIntegrationEventHandler>();
            eventBus.Subscribe<OrderStockConfirmedIntegrationEvent, OrderStockConfirmedIntegrationEventHandler>();
            eventBus.Subscribe<OrderStockRejectedIntegrationEvent, OrderStockRejectedIntegrationEventHandler>();

        }
    }
}
