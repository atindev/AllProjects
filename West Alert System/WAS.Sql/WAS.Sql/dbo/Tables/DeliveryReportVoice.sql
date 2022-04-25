CREATE TABLE [dbo].[DeliveryReportVoice] (
    [Id]                  UNIQUEIDENTIFIER CONSTRAINT [DF_DeliveryReportVoice_Id] DEFAULT (newid()) NOT NULL,
    [SubscriptionId]      UNIQUEIDENTIFIER NOT NULL,
    [NotificationVoiceId] UNIQUEIDENTIFIER NOT NULL,
    [CallId]              VARCHAR (100)    NOT NULL,
    [Status]              VARCHAR (50)     NOT NULL,
    [ToNumber]            VARCHAR (50)     NULL,
    [ErrorMessage]        VARCHAR (500)    NULL,
    [ErrorCode]           INT              NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           VARCHAR (200)     NULL,
    [ModifiedDate]        DATETIME         NULL,
    [ModifiedBy]          VARCHAR (200)     NULL,
    [DeletedDate]         DATETIME         NULL,
    [IsActive]            BIT              NULL,
    CONSTRAINT [PK_DeliveryReportVoice] PRIMARY KEY CLUSTERED ([Id] ASC)
);





