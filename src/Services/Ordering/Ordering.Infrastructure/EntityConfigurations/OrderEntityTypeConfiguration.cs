using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations
{
    class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", OrderingContext.DEFAULT_SCHEMA);
            builder.HasKey(e => e.Id);
            builder.Ignore(e => e.DomainEvents);
            builder.Property(e => e.Id).ForSqlServerUseSequenceHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);
        
            //configuring address which is a value object
            builder.OwnsOne(e => e.Address);

            //configuring private fields
            //builder.Property<System.DateTime>("orderDate").HasColumnName("OrderDate").IsRequired();
            builder.Property<string>("description").HasColumnName("Description").IsRequired(false);

            //configuring navigation properties (that does not exist!)
            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<Domain.AggregatesModel.BuyerAggregate.Buyer>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(e=>e.BuyerId);

            //backing fields need no extra configuration since they are following conventions (_fieldName)

            //Enums can be configured in a specific way:
            var converter = new EnumToNumberConverter<OrderState,short>();
            builder
                .Property(e => e.OrderState)
                .HasConversion(converter);
        }
    }
}
