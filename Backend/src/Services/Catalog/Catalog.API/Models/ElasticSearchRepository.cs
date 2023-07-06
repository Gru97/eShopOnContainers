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
            var response=await elasticClient.SearchAsync<CatalogItem>(s => s
            .Query(q => q
            .MultiMatch(c => c
            .Fields(t => t.Field(m => m.Name)
            .Field(m => m.CatalogBrand.Brand)
            .Field(m => m.CatalogType.Type))
            .Query(Phrase)
            .Fuzziness(Fuzziness.AutoLength(1,5))) || 
            q.Term(x => x.Description,Phrase)));

            return response.Documents;
        }

        public async Task<IEnumerable<CatalogItem>> SearchAsync(SearchModel model)
        {

            //var response = await elasticClient.SearchAsync<CatalogItem>(s => s
            //            .Query(q => q
            //                .Term(x => x.CatalogBrandId, model.BrandId.ToString()) && q
            //                .Term(x => x.CatalogTypeId, model.TypeId.ToString())));


            
            var response = await elasticClient.SearchAsync<CatalogItem>(s => s
                        .Query(q => q
                        .Terms(x=>x
                        .Field(m=>m.CatalogBrandId)
                        .Terms(model.BrandId)) && q                        
                        .Terms(m=>m
                        .Field(d=>d.CatalogTypeId)
                        .Terms(model.TypeId)) && q
                        .Match(c => c
                        .Field(e => e.Name)
                        .Query(model.Name)) && q
                                        .TermRange(k=>k.Field(e=>e.Price)
                                        .LessThanOrEquals(model.PriceTo.ToString())) && q
                                        .TermRange(k => k.Field(l=>l.Price)
                                        .GreaterThanOrEquals(model.PriceFrom.ToString()))));

            return response.Documents;
            
        }

        public async Task<IEnumerable<CatalogItem>> SearchByCatalogNameAsync(string Phrase)
        {
            //var response1 = elasticClient.Search<CatalogItem>(s => s
            //    .Query(q => 
            //    q.QueryString(d => d.Query(phrase))).From(0).Size(5));

            var response =await elasticClient.SearchAsync<CatalogItem>(s => s
            .Query(q => q
                        .Match(c => c
                                    .Field(e => e.Name).Query(Phrase))));
            return response.Documents; 
        }

        public async Task<bool> UpdateAsync(int id, dynamic updated)
        {
            var result = await elasticClient.UpdateAsync<CatalogItem,object>(id,
                e=>e.Index("catalogitem")
                .Doc(updated));

           return result.IsValid;
        }
    }
}
