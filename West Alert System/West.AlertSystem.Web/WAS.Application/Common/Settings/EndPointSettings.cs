namespace WAS.Application.Common.Settings
{
    public static class EndPointSettings
    {
        //Subscription
        public static string CreateSubscription { get; } = $"api/v1/Subscription";

        public static string EditSubscription { get; } = $"api/v1/Subscription/EditSubscription";

        public static string GetSubscriptionByMail { get; } = $"api/v1/Subscription/GetByMail";

        public static string GetByMailUnMasked { get; } = $"api/v1/Subscription/GetByMailUnMasked";

        public static string Unsubscribe { get; } = $"api/v1/Subscription/Unsubscribe";

        public static string GetSubscriptions { get; } = $"api/v1/Subscription/GetSubscriptions";

        public static string VerifyMail { get; } = $"api/v1/Subscription/VerifyMail";

        public static string UnsubscribeEmail { get; } = $"api/v1/Subscription/UnsubscribeEmail";

        public static string GetSubscriberById { get; } = $"api/v1/Subscription/GetSubscriberById";

        public static string DeleteSubscription { get; } = $"api/v1/Subscription/DeleteSubscription";

        public static string GetAllEmployeeType { get; } = $"api/v1/Subscription/GetAllEmployeeType";

        public static string GetAllEmployeeGroup { get; } = $"api/v1/Subscription/GetAllEmployeeGroup";

        public static string GetAllJobTitle { get; } = $"api/v1/Subscription/GetAllJobTitle";

        public static string BlockUser { get; } = $"api/v1/Subscription/BlockUser";

        public static string CheckforBlockedUser { get; } = $"api/v1/Subscription/CheckforBlockedUser";

        public static string SubscriptionFeedback { get; } = $"api/v1/Subscription/SubscriptionFeedback";

        //Location
        public static string GetAllLocations { get; } = $"api/v1/Location";

        //Language
        public static string GetAllLanguages { get; } = $"api/v1/Language";

        //Notification
        public static string CreateNotification { get; } = $"api/v1/Notification";

        public static string GetNotificationCount { get; } = $"api/v1/Notification/GetNotificationCount";

        public static string ApproveReject { get; } = $"api/v1/Notification/ApproveReject";

        public static string GetByStatus { get; } = $"api/v1/Notification/GetByStatus";

        public static string GetNotificationById { get; } = $"api/v1/Notification/GetById";

        public static string GetPagedNotification { get; } = $"api/v1/Notification/GetPagedNotification";

        public static string GetAttachment { get; } = $"api/v1/Notification/GetAttachment";

        public static string GetLatestDeliveryStatus { get; } = $"api/v1/Notification/GetLatestDeliveryStatus";

        public static string GetWhatsAppTemplates { get; } = $"api/v1/Notification/GetWhatsAppTemplates";

        public static string GetFailedNotificationDetails { get; } = $"api/v1/Notification/GetFailedNotificationDetails";


        //Events
        public static string CreateUpdateEvent { get; } = $"api/v1/Event";

        public static string ArchiveEvent { get; } = $"api/v1/Event";

        public static string GetEventById { get; } = $"api/v1/Event";

        public static string GetAllEvents { get; } = $"api/v1/Event";

        public static string GetEventCount { get; } = $"api/v1/Event/GetEventCount";

        public static string GetActiveEvent { get; } = $"api/v1/Event/GetActiveEvent";

        public static string GetTypeAndUrgency { get; } = $"api/v1/Event/GetTypeAndUrgency";


        //Group
        public static string GetAllGroups { get; } = $"api/v1/Group/GetAllGroups";

        public static string CreateUpdateGroup { get; } = $"api/v1/Group/CreateUpdate";

        public static string AddSubscription { get; } = $"api/v1/Group/AddSubscription";

        public static string GetSubscriptionsByGroup { get; } = $"api/v1/Group/GetSubscriptionsByGroup";

        public static string GetDistinctSubscriptionCount { get; } = $"api/v1/Group/GetDistinctSubscriptionCount";

        public static string DeleteGroup { get; } = $"api/v1/Group/DeleteGroup";

        public static string RemoveSubscription { get; } = $"api/v1/Group/RemoveSubscription";

        public static string RestoreGroup { get; } = $"api/v1/Group/RestoreGroup";


        // Shift
        public static string GetAllShifts { get; } = $"api/v1/Shift";

        //City
        public static string GetAllCity { get; } = $"api/v1/City";

        //State
        public static string GetAllState { get; } = $"api/v1/State";

        //Country
        public static string GetAllCountry { get; } = $"api/v1/Country";

        //Department
        public static string GetAllDepartments { get; } = $"api/v1/Department";


        //Dashboard
        public static string GetDashboard { get; } = $"api/v1/Dashboard/GetDashboard";


        //Feedback

        public static string SubmitFeedback { get; } = $"api/v1/Feedback/SubmitFeedback";


        //Template
        public static string CreateTemplate { get; } = $"api/v1/Template/CreateTemplate";

        public static string GetAllTemplates { get; } = $"api/v1/Template";

        public static string GetTemplateById { get; } = $"api/v1/Template/GetTemplateById";

        public static string GetAllCategories { get; } = $"api/v1/Template/GetAllTemplateCategories";

        public static string CreateCategory { get; } = $"api/v1/Template/CreateCategory";


        //IncomingMessages
        public static string GetAllIncomingMessages { get; } = $"api/v1/IncomingMessage";

        public static string GetAudio { get; } = $"api/v1/IncomingMessage/GetAudio";

        public static string GetIncomingMessageById { get; } = $"api/v1/IncomingMessage/GetById";

        //GetVideoUrl
        public static string GetVideo { get; } = $"api/V1/Training/GetVideo";

        public static string GetVideoById { get; } = $"api/V1/Training/GetVideoById";

        //Report
        public static string GetReports { get; } = $"api/v1/Reports/GetReports";

        //Survey
        public static string CreateUpdateSurvey { get; } = $"api/v1/Survey/CreateUpdateSurvey";

        public static string GetAllSurvey { get; } = $"api/v1/Survey/GetAllSurvey";

        public static string DeleteSurvey { get; } = $"api/v1/Survey/DeleteSurvey";

        public static string ShareSurvey { get; } = $"api/v1/Survey/ShareSurvey";

        public static string GetAllShareSurvey { get; } = $"api/v1/Survey/GetAllShareSurvey";

        public static string GetSurveyById { get; } = $"api/v1/Survey/GetSurveyById";

        public static string GetBroadcastView { get; } = $"api/v1/Survey/GetBroadcastView";

        public static string BroadcastSurvey { get; } = $"api/v1/Survey/BroadcastSurvey";
       
        public static string CheckSurveyExpiry { get; } = $"api/v1/Survey/CheckSurveyExpiry";

        public static string GetByBroadcastId { get; } = $"api/v1/Survey/GetByBroadcastId";

        public static string GetAllBroadcast { get; } = $"api/v1/Survey/GetAllBroadcast";

        public static string DeleteBroadcastedSurvey { get; } = $"api/v1/Survey/DeleteBroadcastedSurvey";

        public static string UpdateBroadcastedSurvey { get; } = $"api/v1/Survey/UpdateBroadcastedSurvey";

        public static string CheckAudience { get; } = $"api/v1/Survey/CheckAudience";

        public static string GetJsonByBroadcastId { get; } = $"api/v1/Survey/GetJsonByBroadcastId";
        
        public static string SubmitSurvey { get; } = $"api/v1/Survey/SubmitSurvey";
        
        public static string IsAlreadyFilled { get; } = $"api/v1/Survey/IsAlreadyFilled";

        public static string GetSubmissionReportByLocation { get; } = $"api/v1/Survey/GetSubmissionReportByLocation";
        
        public static string GetSubmissionReportByDepartment { get; } = $"api/v1/Survey/GetSubmissionReportByDepartment";

        public static string GetAnswerwiseReport { get; } = $"api/v1/Survey/GetAnswerwiseReport";

        public static string CloneSurvey { get; } = $"api/v1/Survey/CloneSurvey";

        public static string GetAllSharedPeopleNamesById { get; } = $"api/v1/Survey/GetAllSharedPeopleNamesById";

        public static string GetSubjectByBroadcastId { get; } = $"api/v1/Survey/GetSubjectByBroadcastId";

        public static string GetCreatedByList { get; } = $"api/v1/Survey/GetCreatedByList";

        public static string ExtractKeyPhrasesFromShortAnswer { get; } = $"api/v1/Survey/ExtractKeyPhrasesFromShortAnswer";


        //ocr Subscriptions

        public static string GetOcrSubscriptionList { get; } = $"api/v1/Subscription/GetOcrSubscriptionList";

        public static string GetOcrSubscriptionById { get; } = $"api/v1/Subscription/GetOcrSubscriptionById";

        public static string DiscardOcrSubscription { get; } = $"api/v1/Subscription/DiscardOcrSubscription";
    }
}
