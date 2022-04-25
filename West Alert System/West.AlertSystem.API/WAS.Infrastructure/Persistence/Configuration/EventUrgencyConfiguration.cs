using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class EventUrgencyConfiguration : EntityConfiguration<EventUrgency>
    {
        public override void Configure(EntityTypeBuilder<EventUrgency> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.Events)
                .WithOne(e => e.EventUrgency)
                .HasForeignKey(e => e.UrgencyId);
        }
    }
}
