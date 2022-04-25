using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class SubscriptionConfiguration : EntityConfiguration<Subscription>
    {
        public override void Configure(EntityTypeBuilder<Subscription> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.SubscriptionGroups)
                .WithOne(e => e.Subscription)
                .HasForeignKey(e => e.SubscriptionId);

            builder.HasMany(e => e.DeliveryReportTexts)
               .WithOne(e => e.Subscription)
               .HasForeignKey(e => e.SubscriptionId);

            builder.HasMany(e => e.DeliveryReportWhatsApps)
               .WithOne(e => e.Subscription)
               .HasForeignKey(e => e.SubscriptionId);

            builder.HasMany(e => e.DeliveryReportVoices)
              .WithOne(e => e.Subscription)
              .HasForeignKey(e => e.SubscriptionId);

            builder.HasMany(e => e.SubscriptionFeedbacks)
              .WithOne(e => e.Subscription)
              .HasForeignKey(e => e.SubscriptionId);

            builder.HasMany(e => e.SurveyBroadcastSubscriptions)
              .WithOne(e => e.Subscription)
              .HasForeignKey(e => e.SubscriptionId);
        }
    }
}
