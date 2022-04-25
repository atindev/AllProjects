CREATE TABLE [dbo].[Notification] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [EventId]               UNIQUEIDENTIFIER NOT NULL,
    [NotificationTypeId]    INT              NOT NULL,
    [ScheduledTime]         DATETIME         NULL,
    [TimeZone]              VARCHAR (500)    NULL,
    [TimeZoneOffset]        INT              CONSTRAINT [DF_Notification_TimeZoneOffset] DEFAULT ((0)) NOT NULL,
    [IsText]                BIT              CONSTRAINT [DF_Notification_IsText] DEFAULT ((0)) NOT NULL,
    [IsEmail]               BIT              CONSTRAINT [DF_Notification_IsEmail] DEFAULT ((0)) NOT NULL,
    [IsVoice]               BIT              CONSTRAINT [DF_Notification_IsVoice] DEFAULT ((0)) NOT NULL,
    [IsTeams]               BIT              CONSTRAINT [DF_Notification_IsTeams] DEFAULT ((0)) NOT NULL,
    [IsWhatsApp]            BIT              CONSTRAINT [DF_Notification_IsWhatsApp] DEFAULT ((0)) NOT NULL,
    [Status]                INT              NOT NULL,
    [Comment]               VARCHAR (500)    NULL,
    [ApprovedDate]          DATETIME         NULL,
    [ApprovedTimeZone]      VARCHAR (500)    NULL,
    [ApprovedBy]            VARCHAR (200)     NULL,
    [FinalApprovalDate]     DATETIME         NULL,
    [FinalApprovalTimeZone] VARCHAR (500)    NULL,
    [FinalApprovalBy]       VARCHAR (200)     NULL,
    [FinalComment]          VARCHAR (500)    NULL,
    [SentDate]              DATETIME         NULL,
    [SentTimeZone]          VARCHAR (500)    NULL,
    [CreatedTimeZone]       VARCHAR (500)    NULL,
    [IsSignatureRequired]   BIT              DEFAULT ((0)) NOT NULL,
    [IsApprovalRequired]    BIT              NULL,
    [IsPrivateNotification] BIT              NOT NULL DEFAULT ((0)),
    [ApproverForPrivate]    VARCHAR (200)     NULL,
    [CreatedDate]           DATETIME         NOT NULL,
    [CreatedBy]             VARCHAR (200)     NULL,
    [ModifiedDate]          DATETIME         NULL,
    [ModifiedTimeZone]      VARCHAR (500)    NULL,
    [ModifiedBy]            VARCHAR (200)     NULL,
    [DeletedDate]           DATETIME         NULL,
    [IsActive]              BIT              NULL,
    CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED ([Id] ASC)
);















