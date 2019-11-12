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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IOrderRepository,OrderRepository>();
            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

            

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

            app.UseHttpsRedirection();
            app.UseMvc();
            
            //ConfigureEventBus(app);

        }
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<UserCheckoutIntegrationEvent, UserCheckoutIntegrationEventHandler>();
            //eventBus.Subscribe<UserCheckoutIntegrationEvent, IIntegrationEventHandler<UserCheckoutIntegrationEvent>>();

        }
    }
}
