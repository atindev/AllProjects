using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class NotificationTextConfiguration: EntityConfiguration<NotificationText>
    {
        public override void Configure(EntityTypeBuilder<NotificationText> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.DeliveryReportTexts)
                .WithOne(e => e.NotificationText)
                .HasForeignKey(e => e.NotificationTextId);
        }
    }
}
