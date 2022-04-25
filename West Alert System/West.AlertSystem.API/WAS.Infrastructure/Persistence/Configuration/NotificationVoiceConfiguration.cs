using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class NotificationVoiceConfiguration: EntityConfiguration<NotificationVoice>
    {
        public override void Configure(EntityTypeBuilder<NotificationVoice> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.DeliveryReportVoices)
                .WithOne(e => e.NotificationVoice)
                .HasForeignKey(e => e.NotificationVoiceId);
        }
    }
}
