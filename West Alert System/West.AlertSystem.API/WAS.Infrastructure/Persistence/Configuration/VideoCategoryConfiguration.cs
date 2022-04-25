
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
   public class VideoCategoryConfiguration : EntityConfiguration<VideoCategory>
   {
        public override void Configure(EntityTypeBuilder<VideoCategory> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.TrainingVideos)
                .WithOne(e => e.VideoCategory)
                .HasForeignKey(e => e.CategoryId);



            builder.HasOne(e => e.Language)
                    .WithMany(lan => lan.VideoCategories)
                    .HasForeignKey(e => e.LanguageCode)
                    .HasPrincipalKey(lan => lan.Code);
        }
    }
}
