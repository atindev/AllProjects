using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class EventTypeConfiguration : EntityConfiguration<EventType>
    {
        public override void Configure(EntityTypeBuilder<EventType> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.Events)
                .WithOne(e => e.EventType)
                .HasForeignKey(e => e.TypeId);
        }
    }
}
