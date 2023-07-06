using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure.EntityConfigurations
{
    class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems", OrderingContext.DEFAULT_SCHEMA);
            builder.HasKey(e => e.Id);
            builder.Ignore(e => e.DomainEvents);
            builder.Property(e => e.Id).ForSqlServerUseSequenceHiLo("orderitemseq");
            builder.Property(e => e.ProductName).HasField("_productName").IsRequired();
        }
    }
}
