using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Settings
{
    public class AzureStorageSettings
    {
        public string StorageConnectionString { get; set; }
        public string StorageAccountUrl { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageKey { get; set; }
        public string SasToken { get; set; }
        public string EmailAttachmentContainer { get; set; }
        public string EmailFormatContainer { get; set; }
        public string EmailNotificationQueue { get; set; }
        public string WorkEmailSendGridTemplateID { get; set; }
        public string EmailSendGridTemplateID { get; set; }
        public string SmsNotificationQueue { get; set; }
        public string SmsSendGridTemplateID { get; set; }
        public string VoiceNotificationQueue { get; set; }
        public string WhatsAppNotificationQueue { get; set; }
        public string UserFeedbackQueue { get; set; }
        public string SubscriptionConfirmationQueue { get; set; }
        public string SubscriptionConfirmationTemplateID { get; set; }
        public string SendEmailQueue { get; set; }
        public string WorkEmailTemplate { get; set; }
        public string PersonalEmailTemplate { get; set; }
        public string SubscriptionConfirmationTemplate { get; set; }
        public string AlertAdminTemplate { get; set; }
        public string ShareSurveyTemplate { get; set; }
        public string GroupChangeNotificationTemplate { get; set; }
        public string NotificationApprovalAlertTemplate { get; set; }
        public string GroupRemovalNotificationTemplate { get; set; }
        public string BlockedUserNotificationQueue { get; set; }
        public string BlockedUserEmailTemplate { get; set; }
        public string EmailSurveyQueue { get; set; }
        public string SurveyWorkEmailTemplate { get; set; }
        public string SurveyPersonalEmailTemplate { get; set; }
        public string AdminAlertQueue { get; set; }
        public string OcrSubscriptionBlobBaseUrl { get; set; }
        public string SendEmailSurveyQueue { get; set; }
    }
}
