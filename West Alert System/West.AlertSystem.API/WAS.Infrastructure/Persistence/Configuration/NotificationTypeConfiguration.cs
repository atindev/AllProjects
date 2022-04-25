using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class NotificationTypeConfiguration : EntityConfiguration<NotificationType>
    {
        public override void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.Notification)
                .WithOne(e => e.NotificationType)
                .HasForeignKey(e => e.NotificationTypeId);
        }
    }
}
