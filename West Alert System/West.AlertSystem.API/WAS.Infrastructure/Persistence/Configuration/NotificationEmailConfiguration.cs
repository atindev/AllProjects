using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class NotificationEmailConfiguration : EntityConfiguration<NotificationEmail>
    {
        public override void Configure(EntityTypeBuilder<NotificationEmail> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.NotificationEmailAttachments)
                .WithOne(e => e.NotificationEmail)
                .HasForeignKey(e => e.NotificationEmailId);
        }
    }
}
