using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventStore
{
    public class DomainEventLogContext:DbContext
    {
        public DomainEventLogContext(DbContextOptions<DomainEventLogContext> options) : base(options) { }
        public DbSet<DomainEventLogEntry> DomainEventLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DomainEventLogEntry>(ConfigureDomainEventLogEntry);
        }

        private void ConfigureDomainEventLogEntry(EntityTypeBuilder<DomainEventLogEntry> builder)
        {
            builder.ToTable("DomainEventLog");
            builder.HasKey(x => x.EventId);
            builder.Property(e => e.EventId).IsRequired();
            builder.Property(e => e.CreationTime).IsRequired();
            builder.Property(e => e.State).IsRequired();
            builder.Property(e => e.TimeSent).IsRequired();
            builder.Property(e => e.EventTypeName).IsRequired();
;        }
    }
}
