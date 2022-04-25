using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class GroupConfiguration : EntityConfiguration<Group>
    {
        public override void Configure(EntityTypeBuilder<Group> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.SubscriptionGroups)
                .WithOne(e => e.Group)
                .HasForeignKey(e => e.GroupId);

            builder.HasMany(e => e.SurveyBroadcastGroups)
            .WithOne(e => e.Group)
            .HasForeignKey(e => e.GroupId);
        }
    }
}
