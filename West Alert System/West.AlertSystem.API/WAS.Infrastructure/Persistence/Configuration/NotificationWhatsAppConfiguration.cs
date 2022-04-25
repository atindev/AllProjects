using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class NotificationWhatsAppConfiguration : EntityConfiguration<NotificationWhatsApp>
    {
        public override void Configure(EntityTypeBuilder<NotificationWhatsApp> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.DeliveryReportWhatsApps)
                .WithOne(e => e.NotificationWhatsApp)
                .HasForeignKey(e => e.NotificationWhatsAppId);
        }
    }
}
