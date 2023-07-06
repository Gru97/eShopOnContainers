using Catalog.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API
{
    public static class ElasticSearchExtensions
    {

        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<CatalogItem>(m => m
                
                 
                    .PropertyName(c=>c.Id, "id")
                    .PropertyName(c=>c.Name,"name")
                    .PropertyName(c=>c.Description,"description")
                    .PropertyName(c=>c.CatalogBrand,"brand")
                    .PropertyName(c=>c.CatalogType,"type")
                    
                )
                .DefaultMappingFor<CatalogType>(m => m
                    .PropertyName(c => c.Id, "id")
                    .PropertyName(c=>c.Type,"type")
                )
                .DefaultMappingFor<CatalogBrand>(m => m
                    .PropertyName(c => c.Id, "id")
                    .PropertyName(c=>c.Brand,"brand")
                );


            try
            {
                var client = new ElasticClient(settings);
                var createIndexResponse = client.Indices.Create("catalogitem");                 
                services.AddSingleton<IElasticClient>(client);

            }
            catch (Exception ex)
            {

                throw new Exception("Connecting to elastic failed"+ex.Message);
            }

        }
    }
    
}
