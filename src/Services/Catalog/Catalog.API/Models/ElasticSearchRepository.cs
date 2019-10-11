using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models
{
    public class ElasticSearchRepository : ISearchRepository<CatalogItem>
    {
        private readonly IElasticClient elasticClient;
        public ElasticSearchRepository(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            bool result = true;
            var response=await elasticClient.DeleteAsync<CatalogItem>(Id);
            if (!response.IsValid)
                result = false;
            return result;
        }

        public async Task<bool> SaveAsync(CatalogItem entity)
        {
            var response =await elasticClient.IndexDocumentAsync<CatalogItem>(entity);
            bool result = true;

            if (!response.IsValid)
                result = false;
            return result;
        }

        public async Task<bool> SaveManyAsync(IEnumerable<CatalogItem> entites)
        {
            var indexManyResponse =await elasticClient.IndexManyAsync(entites);
            bool result = true;

            if (indexManyResponse.Errors)
            {
                foreach (var item in indexManyResponse.ItemsWithErrors)
                {
                    //log
                }

                result = false;
            }
            return result;
        }

        public async Task<IEnumerable<CatalogItem>> SearchAsync(string Phrase)
        {
            var response=await elasticClient.SearchAsync<CatalogItem>(s => s.Query(q => q.MultiMatch(c => c.Fields(t => t.Field(m => m.Name).Field(m => m.CatalogBrand.Brand).Field(m => m.CatalogType.Type)).Query(Phrase))));
            return response.Documents;
        }

        public async Task<IEnumerable<CatalogItem>> SearchByCatalogNameAsync(string Phrase)
        {
            //var response1 = elasticClient.Search<CatalogItem>(s => s
            //    .Query(q => 
            //    q.QueryString(d => d.Query(phrase))).From(0).Size(5));

            var response =await elasticClient.SearchAsync<CatalogItem>(s => s.Query(q => q.Match(c => c.Field(e => e.Name).Query(Phrase))));
            return response.Documents; 
        }
    }
}
