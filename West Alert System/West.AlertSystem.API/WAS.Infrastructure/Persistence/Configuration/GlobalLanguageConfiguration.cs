using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    class GlobalLanguageConfiguration : EntityConfiguration<GlobalLanguage>
    {
        public override void Configure(EntityTypeBuilder<GlobalLanguage> builder)
        {
            base.Configure(builder);

            builder.HasMany(lang => lang.VideoCategories)
                   .WithOne(vid => vid.Language)
                   .HasForeignKey(vid => vid.LanguageCode);
            
            builder.HasAlternateKey(x => x.Code);
            
        }
    }
}
