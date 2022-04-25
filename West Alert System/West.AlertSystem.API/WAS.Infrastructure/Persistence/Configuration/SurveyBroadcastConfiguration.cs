using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence.Configuration
{
    public class SurveyBroadcastConfiguration : EntityConfiguration<SurveyBroadcast>
    {
        public override void Configure(EntityTypeBuilder<SurveyBroadcast> builder)
        {
            base.Configure(builder);

            builder.HasMany(e => e.SurveyBroadcastSubscriptions)
                .WithOne(e => e.SurveyBroadcast)
                .HasForeignKey(e => e.SurveyBroadcastId);

            builder.HasMany(e => e.SurveyBroadcastGroups)
                .WithOne(e => e.SurveyBroadcast)
                .HasForeignKey(e => e.SurveyBroadcastId);
           
            builder.HasOne(e => e.SurveyBroadcastFollowup)
               .WithOne(e => e.SurveyBroadcast)
               .HasForeignKey<SurveyBroadcastFollowup>(e => e.SurveyBroadcastId);

            builder.HasOne(e => e.SurveyBroadcastEmail)
               .WithOne(e => e.SurveyBroadcast)
               .HasForeignKey<SurveyBroadcastEmail>(e => e.SurveyBroadcastId);

            builder.HasOne(e => e.SurveyBroadcastText)
                 .WithOne(e => e.SurveyBroadcast)
                 .HasForeignKey<SurveyBroadcastText>(e => e.SurveyBroadcastId);

            builder.HasOne(e => e.SurveyBroadcastTeams)
                  .WithOne(e => e.SurveyBroadcast)
                  .HasForeignKey<SurveyBroadcastTeams>(e => e.SurveyBroadcastId);

            builder.HasOne(e => e.SurveyBroadcastWhatsApp)
                 .WithOne(e => e.SurveyBroadcast)
                 .HasForeignKey<SurveyBroadcastWhatsApp>(e => e.SurveyBroadcastId);

            builder.HasMany(e => e.SurveyDetailShare)
                .WithOne(e => e.SurveyBroadcast)
                .HasForeignKey(e => e.BroadcastId);
        }
    }
}
