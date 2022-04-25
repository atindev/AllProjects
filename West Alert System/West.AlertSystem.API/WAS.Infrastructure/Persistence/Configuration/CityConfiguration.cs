using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class CityConfiguration : EntityConfiguration<City>
    {
        public override void Configure(EntityTypeBuilder<City> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.Locations)
                .WithOne(e => e.City)
                .HasForeignKey(e => e.CityId);

        }

    }
}
