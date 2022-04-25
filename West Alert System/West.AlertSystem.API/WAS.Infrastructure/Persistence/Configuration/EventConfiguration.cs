using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class EventConfiguration : EntityConfiguration<Event>
    {
        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.Notifications)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventId);

        }

    }
}
