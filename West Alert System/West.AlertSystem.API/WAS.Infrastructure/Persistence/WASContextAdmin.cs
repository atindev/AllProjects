using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Interface;
using WAS.Domain;
using WAS.Domain.Entities;

namespace WAS.Infrastructure.Persistence
{
    public class WasContextAdmin : DbContext, IWasContextAdmin
    {
        public WasContextAdmin(DbContextOptions<WasContextAdmin> options) : base(options)
        {

        }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationGroup> NotificationGroups { get; set; }

        public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

        public DbSet<NotificationEmail> NotificationEmails { get; set; }

        public DbSet<NotificationEmailAttachment> NotificationEmailAttachments { get; set; }

        public DbSet<NotificationText> NotificationTexts { get; set; }

        public DbSet<NotificationWhatsApp> NotificationWhatsApps { get; set; }

        public DbSet<NotificationVoice> NotificationVoices { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }
        
        public DbSet<DeletedSubscription> DeletedSubscription { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Shift> Shifts { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Country> Countries{ get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<SubscriptionGroup> SubscriptionGroups { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<EventUrgency> EventUrgencies { get; set; }

        public DbSet<DeliveryReportText> DeliveryReportTexts { get; set; }

        public DbSet<DeliveryReportWhatsApp> DeliveryReportWhatsApps { get; set; }

        public DbSet<DeliveryReportVoice> DeliveryReportVoices { get; set; }

        public DbSet<WhatsAppTemplate> WhatsAppTemplates { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<TemplateCategory> TemplateCategories { get; set; }

        public DbSet<IncomingMessage> IncomingMessages { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }

        public DbSet<TrainingVideos> TrainingVideos { get; set; }

        public DbSet<VideoCategory> VideoCategories { get; set; }

        public DbSet<SubscriptionFeedback> SubscriptionFeedbacks { get; set; }

        public DbSet<BlockedUser> BlockedUsers { get; set; }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<SurveyBroadcast> SurveyBroadcasts { get; set; }

        public DbSet<SurveyBroadcastSubscription> SurveyBroadcastSubscriptions { get; set; }

        public DbSet<SurveyBroadcastGroup> SurveyBroadcastGroups { get; set; }

        public DbSet<SurveyBroadcastEmail> SurveyBroadcastEmails { get; set; }

        public DbSet<SurveyBroadcastTeams> SurveyBroadcastTeams { get; set; }

        public DbSet<SurveyBroadcastWhatsApp> SurveyBroadcastWhatsApps { get; set; }

        public DbSet<SurveyBroadcastText> SurveyBroadcastTexts { get; set; }

        public DbSet<SurveyBroadcastFollowup> SurveyBroadcastFollowups { get; set; }

        public DbSet<OcrSubscription> OcrSubscriptions { get; set; }

        public DbSet<GlobalLanguage> GlobalLanguages { get; set; }

        public DbSet<SurveyBroadcastADUser> SurveyBroadcastADUser { get; set; }

        public DbSet<SurveyBroadcastDistributionGroup> SurveyBroadcastDistributionGroup { get; set; }

        public DbSet<SurveyDetailShare> SurveyDetailShare { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        entry.Entity.IsActive = true;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        break;                    
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
