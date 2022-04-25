using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class StateConfiguration : EntityConfiguration<State>
    {
        public override void Configure(EntityTypeBuilder<State> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.Cities)
                .WithOne(e => e.State)
                .HasForeignKey(e => e.StateId);
        }
    }
}
