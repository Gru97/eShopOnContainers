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
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Repositories;

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


            services.AddTransient<IOrderRepository,OrderRepository>();
            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();
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

            services.AddSwaggerGen(ac => {
                ac.SwaggerDoc("ordering", new Swashbuckle.AspNetCore.Swagger.Info { Title="Ordering API",Version="v1"});
                
                var contentRoot = HostingEnvironment.ContentRootPath;
                //var path = $"{contentRoot}\Ordering.API.xml";
                //ac.IncludeXmlComments(path);
            });

            services.AddScoped<DoesBuyerExist, DoesBuyerExist>();
            
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/swagger.json", "Ordering API"));
            //app.UseHttpsRedirection();
            app.UseAuthentication();
           
            app.UseMvc();
            
            ConfigureEventBus(app);

        }
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<UserCheckoutIntegrationEvent, UserCheckoutIntegrationEventHandler>();
            eventBus.Subscribe<OrderStockConfirmedIntegrationEvent, OrderStockConfirmedIntegrationEventHandler>();
            eventBus.Subscribe<OrderStockRejectedIntegrationEvent, OrderStockRejectedIntegrationEventHandler>();
            //eventBus.Subscribe<UserCheckoutIntegrationEvent, IIntegrationEventHandler<UserCheckoutIntegrationEvent>>();

        }
    }
}
