using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class NotificationConfiguration : EntityConfiguration<Notification>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => e.NotificationEmail)
                .WithOne(e => e.Notification)
                .HasForeignKey<NotificationEmail>(e => e.NotificationId);

            builder.HasOne(e => e.NotificationText)
                .WithOne(e => e.Notification)
                .HasForeignKey<NotificationText>(e => e.NotificationId);

            builder.HasOne(e => e.NotificationVoice)
                .WithOne(e => e.Notification)
                .HasForeignKey<NotificationVoice>(e => e.NotificationId);

            builder.HasMany(e => e.NotificationGroups)
                .WithOne(e => e.Notification)
                .HasForeignKey(e => e.NotificationId);

            builder.HasMany(e => e.NotificationSubscriptions)
                .WithOne(e => e.Notification)
                .HasForeignKey(e => e.NotificationId);

            builder.HasOne(e => e.NotificationWhatsApp)
                .WithOne(e => e.Notification)
                .HasForeignKey<NotificationWhatsApp>(e => e.NotificationId);

            builder.HasMany(e => e.IncomingMessages)
                .WithOne(e => e.Notification)
                .HasForeignKey(e => e.NotificationId);
        }
    }
}
