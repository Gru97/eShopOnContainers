﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationEventLogEF
{
    public class IntegrationEventLogContext:DbContext
    {
        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options) { }
        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationEventLogEntry>(ConfigureIntegrationEventLogEntry);
        }

        private void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<IntegrationEventLogEntry> builder)
        {
            builder.ToTable("IntegrationEventLog");
            builder.HasKey(x => x.EventId);
            builder.Property(e => e.EventId).IsRequired();
            builder.Property(e => e.CreationTime).IsRequired();
            builder.Property(e => e.State).IsRequired();
            builder.Property(e => e.TimeSent).IsRequired();
            builder.Property(e => e.EventTypeName).IsRequired();
;        }
    }
}
