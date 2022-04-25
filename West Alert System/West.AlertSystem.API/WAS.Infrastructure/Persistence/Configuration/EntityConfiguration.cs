using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(typeof(T).Name);

            builder.Property(e => e.CreatedDate)
                .IsRequired();

            builder.Property(e => e.ModifiedDate)
                .IsRequired(false);

            builder.Property(e => e.DeletedDate)
                .IsRequired(false);

            builder.HasQueryFilter(e => e.IsActive);
        }
    }
}
