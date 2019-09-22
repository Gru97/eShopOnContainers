using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildingBlocks.EventBusRabbitMQ;
using Catalog.API.Infrastructure;
using Catalog.API.IntegrationEvents;
using EventBus.Abstractions;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.API
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
            services.AddDbContext<CatalogContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("CatalogContext"));
                },ServiceLifetime.Scoped);
            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                
                options.UseSqlServer(Configuration.GetConnectionString("CatalogContext"),
                    sqlServerOptionsAction:sqlOptions=>sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
            } ,ServiceLifetime.Scoped);

            RegisterEventBus(services);
            services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();
            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>(sp =>
            {
                return new IntegrationEventLogService(Configuration.GetConnectionString("CatalogContext"));
            });
        }
        private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];


            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                return new EventBusRabbitMQ(subscriptionClientName);
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

  
            app.UseMvc();
        }
    }
}
