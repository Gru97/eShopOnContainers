using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EventStore;
using Microsoft.Extensions.Logging;
using Ordering.Domain;
using Ordering.QueryModel;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Infrastructure.Reporting.Repositories
{
    public class MongoRepository : IOrderQueries
    {
        private readonly IMongoClient mongoClient;
        private readonly IMongoDatabase database;
        protected IMongoCollection<OrderDocument> collection;
        private readonly QueryModelConfiguration connectionConfig;
        private readonly ILogger<MongoRepository>  logger;



        public MongoRepository(QueryModelConfiguration connection, ILogger<MongoRepository> logger)
        {
            connectionConfig = connection;
            this.logger = logger;
            var connectionString = connectionConfig.ConnectionString;
            mongoClient = new MongoClient(connectionString);
            database = mongoClient?.GetDatabase("eshop");
            collection = database?.GetCollection<OrderDocument>(OrderDocument.DocumentName);
        }

        public async Task Upsert(OrderDocument order)
        {
            var existing = await GetOrderAsync(order.OrderId);
            var b=new BsonDocument("$set",order.ToBsonDocument());
                try
                {
                    if (existing != null)
                        await collection?.UpdateOneAsync<OrderDocument>
                            (e => e.OrderId == order.OrderId, b);
                    else
                        await collection?.InsertOneAsync(order);
                }
                catch (Exception ex)
                {
                    logger.LogError("$Error while upsering doc in Mongo " + ex.Message);
                    throw  new Exception(ex.Message );
                }
       
        }

        public async Task<OrderSummeryViewModel> GetOrderAsync(int orderId)
        {
            //var filter = new BsonDocument() {{"orderId", orderId}};
            try
            {
                var documents = await collection?.Find(e => e.OrderId == orderId)?.FirstOrDefaultAsync();
                    
                if (documents != null)
                    return ToOrderSummeryViewModel(documents);
                return null;

            }
            catch (Exception ex)
            {
                logger.LogError("$Method:GetOrderAsync - Error while querying doc from Mongo " + ex.Message);
                throw new Exception(ex.Message);
            }
          
        }


      
        public async Task<OrderViewModel> GetOrderDetails(int orderId)
        {
            try
            {
                var document = await collection.Find(e=>e.OrderId==orderId)?.FirstOrDefaultAsync();
                return ToOrderViewModel(document);
            }
            catch (Exception ex)
            {
                logger.LogError("$Method:GetOrderDetails - Error while querying doc from Mongo " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        private OrderViewModel ToOrderViewModel(OrderDocument order)
        {
            string seperator = "-";

            return new OrderViewModel()
            {
                address = order.Address.Country + seperator
                        + order.Address.City + seperator
                        + order.Address.Street + seperator
                        + order.Address.State + seperator
                        + order.Address.ZipCode,
                orderitems = order.OrderItems.Select(e => ToOrderItemViewModel(e)).ToList(),
                buyername = order.BuyerInfo.BuyerName,
                date = order.OrderDate,
                ordernumber = order.OrderId,
                total = order.OrderItems.Sum(e => e.Quantity * e.UnitPrice)
            };
        }

        public async Task<PagedResult<OrderSummeryViewModel>> GetOrders(int pageSize, int pageIndex)
        {
            return await GetPagedResultAsync(s=>true, pageSize, pageIndex);
        }

        public async Task<PagedResult<OrderSummeryViewModel>> GetOrdersByStatus(int pageSize, int pageIndex, int status)
        {
            return await GetPagedResultAsync(s=>s.Status==status, pageSize,pageIndex);
        }

        private async Task<PagedResult<OrderSummeryViewModel>> GetPagedResultAsync(Expression<Func<OrderDocument,bool>> predicate, int pageSize, int pageIndex)
        {
            try
            {
                var resultDocs = await collection?.Find(predicate)?.ToListAsync();
                var count = resultDocs.Count();
                var paged = resultDocs.Skip(pageSize * pageIndex).Take(pageSize).ToList();
                var items = ToOrderSummeryViewModel(paged);
                return new PagedResult<OrderSummeryViewModel>() { Count = count, Items = items };
            }
            catch (Exception ex)
            {
                logger.LogError("$Method: - Error while upsering querying from Mongo " + ex.Message);
                throw new Exception(ex.Message);
            }
        
        }

        public async Task<List<OrderSummeryViewModel>> GetOrdersForBuyer(string buyerId)
        {
            try
            {
                var result = await collection?.Find(e => e.BuyerInfo.BuyerGuid == buyerId)?.ToListAsync();
                return ToOrderSummeryViewModel(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method:GetOrdersForBuyer - Error while querying doc from Mongo " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private List<OrderSummeryViewModel>ToOrderSummeryViewModel(List<OrderDocument> o)
        {
            return o.Select(ToOrderSummeryViewModel).ToList();
        }
        private OrderSummeryViewModel ToOrderSummeryViewModel(OrderDocument o)
        {
            return new OrderSummeryViewModel
            {
                ordernumber = o.OrderId,
                total = o.OrderItems.Sum(e => e.UnitPrice * e.Quantity),
                date = o.OrderDate,
                status = o.Status.ToString()
            };
        }
        private OrderItemViewModel ToOrderItemViewModel(OrderItem e)
        {
            return new OrderItemViewModel()
            {
                productname = e.ProductName,
                unitprice = e.UnitPrice,
                units = e.Quantity
            };
        }

        public async Task<OrderDocument> GetOrderDocument(int orderId)
        {
            try
            {
                var documents = await collection?.Find(e => e.OrderId == orderId)?.FirstOrDefaultAsync();

                if (documents != null)
                    return documents;
                return null;

            }
            catch (Exception ex)
            {
                logger.LogError("$Method:GetOrderAsync - Error while querying doc from Mongo " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
