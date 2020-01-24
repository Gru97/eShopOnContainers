using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Domain;
using Ordering.QueryModel;
using Remotion.Linq;
using Buyer = Ordering.Domain.AggregatesModel.BuyerAggregate.Buyer;
using OrderItem = Ordering.Domain.AggregatesModel.OrderAggregate.OrderItem;

namespace Ordering.Infrastructure
{
    public class OrderingContext:DbContext,IUnitOfWork
    {
        public static string DEFAULT_SCHEMA { get; internal set; }
        private readonly IMediator mediator;
        private readonly IDomainEventLogService eventStore;

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        


        public OrderingContext(DbContextOptions<OrderingContext> options, IMediator mediator,  IOrderQueries orderQueries, IDomainEventLogService eventStore) : base(options)
        {
            this.mediator = mediator;
            this.eventStore = eventStore;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
        

        }
        public async Task<int> SaveChangesAsync()
        {
            //dispatch domain events before pesisting data
            await DispatchDomainEventsAsync(this, mediator);
            using (var transaction = base.Database.BeginTransaction())
            {
                //SaveChangesOnQueryModel(this);
                await base.SaveChangesAsync();
                transaction.Commit();
            }

            return 0;



        }

        //private void SaveChangesOnQueryModel(OrderingContext ctx)
        //{
        //    var order = ctx.ChangeTracker.Entries<Order>()?.FirstOrDefault()?.Entity;
        //    var buyer = ctx.ChangeTracker.Entries<Buyer>()?.FirstOrDefault()?.Entity;
        //    var items = new List<QueryModel.OrderItem>();
        //    if (order!=null)
        //    {
        //        foreach (var x in order.OrderItems)
        //        {
        //            items.Add(new QueryModel.OrderItem()
        //            {
        //                Discount = x.Discount,
        //                ProductId = x.ProductId,
        //                ProductName = x.ProductName,
        //                Quantity = x.Quantity,
        //                UnitPrice = x.UnitPrice
        //            });
        //        }
              

        //        var doc = new OrderDocument()
        //        {
                    
        //            OrderDate = order.OrderDate,
        //            OrderId = order.Id,
        //            Status = (short)order.OrderState,
        //            Address = new QueryModel.Address()
        //            {
        //                City = order.Address.City,
        //                Country = order.Address.Country,
        //                State = order.Address.State,
        //                Street = order.Address.Street,
        //                ZipCode = order.Address.ZipCode
        //            },
        //            OrderItems = items


        //        };
        //        if (buyer != null)
        //            doc.BuyerInfo = new QueryModel.Buyer() {BuyerName = buyer.Name, BuyerId = buyer.IdentityGuid};
        //        else
        //        {
        //            var untrackedBuyer= ctx.Buyers.First(e => e.Id == order.BuyerId);
                    
        //            doc.BuyerInfo = new QueryModel.Buyer() { BuyerName = untrackedBuyer.Name, BuyerId = untrackedBuyer.IdentityGuid };

                    
        //        }
        //        orderQueries.Upsert(doc);
        //    }
           

        //}

        public async Task DispatchDomainEventsAsync(OrderingContext ctx,IMediator mediator)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());


            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEvents.OrderBy(e => e.CreationDate)
                .ToList()
                .ForEach(entity => eventStore.SaveEventAsync(entity).Wait());

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }

        private DomainEvent ToDomainEvent(DomainEvent entity)
        {
            var @event=new DomainEvent(Guid.NewGuid() , entity.CreationDate);
            return @event;
        }
    }

    
}
