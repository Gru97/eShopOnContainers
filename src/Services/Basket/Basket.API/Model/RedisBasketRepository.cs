using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.API.Model
{
    public class RedisBasketRepository:IBasketRepository
    {
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase database;

        public RedisBasketRepository(ConnectionMultiplexer redis)
        {
            this.redis = redis;
            this.database = redis.GetDatabase();
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data= await database.StringGetAsync(customerId);
            return JsonConvert.DeserializeObject<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            await database.StringSetAsync(basket.CustomerId, JsonConvert.SerializeObject(basket));
            return await GetBasketAsync(basket.CustomerId);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            bool result = await database.KeyDeleteAsync(id);
            return result;
            ;
        }
    }
}
