using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class TemplateCategoryConfiguration : EntityConfiguration<TemplateCategory>
    {
        public override void Configure(EntityTypeBuilder<TemplateCategory> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.Templates)
                .WithOne(e => e.TemplateCategory)
                .HasForeignKey(e => e.CategoryId);
        }
    }
}
