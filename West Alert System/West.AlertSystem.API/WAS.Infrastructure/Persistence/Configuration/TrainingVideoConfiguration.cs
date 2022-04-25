
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
   public class TrainingVideoConfiguration : EntityConfiguration<TrainingVideos>
    {
        public override void Configure(EntityTypeBuilder<TrainingVideos> builder)
        {
            base.Configure(builder);


            builder.HasOne(e => e.Language)
                    .WithMany(lan => lan.TrainingVideos)
                    .HasForeignKey(e => e.LanguageCode)
                    .HasPrincipalKey(lan => lan.Code);
        }
    }
}
