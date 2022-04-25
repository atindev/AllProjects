CREATE TABLE [dbo].[CommunicationPreference] (
    [Id]                INT    NOT NULL,
    [SubscriptionID]    BIGINT NOT NULL,
    [Preference]        INT    NOT NULL,
    [CommunicationType] INT    NOT NULL,
    CONSTRAINT [PK_CommunicationPreference] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'OfficeEmail-1
PersonalEmail-2
VoiceOfficeMobile-3
VoicePersonalMobile-4
VoiceOfficePhone-5
VoiceHomePhone-6
TextOfficeMobile-7
TextPersonalMobile-8
WhatsAppOfficeMobile-9
WhatsAppPersonalMobile-10
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CommunicationPreference', @level2type = N'COLUMN', @level2name = N'Preference';

