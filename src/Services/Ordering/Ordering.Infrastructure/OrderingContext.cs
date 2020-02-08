using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            try
            {
                await DispatchDomainEventsAsync(this, mediator);
                await base.SaveChangesAsync();
            }
            catch (Exception e)
            {
                
            }
            return 0;



        }

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
