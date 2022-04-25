using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Domain.Entities;

namespace WAS.Application.Interface
{
    public interface IWasContext
    {
        DbSet<Notification> Notifications { get; set; }

        DbSet<NotificationGroup> NotificationGroups { get; set; }

        DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

        DbSet<NotificationEmail> NotificationEmails { get; set; }

        DbSet<NotificationEmailAttachment> NotificationEmailAttachments { get; set; }

        DbSet<NotificationText> NotificationTexts { get; set; }

        DbSet<NotificationWhatsApp> NotificationWhatsApps { get; set; }

        DbSet<NotificationVoice> NotificationVoices { get; set; }

        DbSet<Subscription> Subscriptions { get; set; }

        DbSet<DeletedSubscription> DeletedSubscription { get; set; }

        DbSet<Location> Locations { get; set; }

        DbSet<City> Cities { get; set; }

        DbSet<State> States { get; set; }

        DbSet<Country> Countries { get; set; }

        DbSet<Language> Languages { get; set; }

        DbSet<Department> Departments { get; set; }

        DbSet<Shift> Shifts { get; set; }

        DbSet<Group> Groups { get; set; }

        DbSet<SubscriptionGroup> SubscriptionGroups { get; set; }

        DbSet<Event> Events { get; set; }

        DbSet<EventType> EventTypes { get; set; }

        DbSet<EventUrgency> EventUrgencies { get; set; }

        DbSet<DeliveryReportText> DeliveryReportTexts { get; set; }

        DbSet<DeliveryReportWhatsApp> DeliveryReportWhatsApps { get; set; }

        DbSet<DeliveryReportVoice> DeliveryReportVoices { get; set; }

        DbSet<WhatsAppTemplate> WhatsAppTemplates { get; set; }

        DbSet<Template> Templates { get; set; }

        DbSet<TemplateCategory> TemplateCategories { get; set; }

        DbSet<IncomingMessage> IncomingMessages { get; set; }

        DbSet<NotificationType> NotificationTypes { get; set; }

        DbSet<TrainingVideos> TrainingVideos { get; set; }

        DbSet<VideoCategory> VideoCategories { get; set; }

        DbSet<SubscriptionFeedback> SubscriptionFeedbacks { get; set; }

        DbSet<BlockedUser> BlockedUsers { get; set; }

        DbSet<Survey> Surveys { get; set; }

        DbSet<SurveyBroadcast> SurveyBroadcasts { get; set; }

        DbSet<SurveyBroadcastSubscription> SurveyBroadcastSubscriptions { get; set; }

        DbSet<SurveyBroadcastGroup> SurveyBroadcastGroups { get; set; }

        DbSet<SurveyBroadcastDistributionGroup> SurveyBroadcastDistributionGroup { get; set; }

        DbSet<SurveyBroadcastADUser> SurveyBroadcastADUser { get; set; }

        DbSet<SurveyBroadcastEmail> SurveyBroadcastEmails { get; set; }

        DbSet<SurveyBroadcastTeams> SurveyBroadcastTeams { get; set; }

        DbSet<SurveyBroadcastWhatsApp> SurveyBroadcastWhatsApps { get; set; }

        DbSet<SurveyBroadcastText> SurveyBroadcastTexts { get; set; }
       
        DbSet<SurveyBroadcastFollowup> SurveyBroadcastFollowups { get; set; }

        DbSet<OcrSubscription> OcrSubscriptions { get; set; }
        DbSet<GlobalLanguage> GlobalLanguages { get; set; }
        DbSet<SurveyDetailShare> SurveyDetailShare { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

        EntityEntry Entry(object entity);
    }

    public interface IDbContext : IDisposable
    {
        DbContext Instance { get; }
    }
}
