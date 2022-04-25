CREATE TABLE [dbo].[DeliveryReportWhatsApp] (
    [Id]                     UNIQUEIDENTIFIER CONSTRAINT [DF_DeliveryReportWhatsApp_Id] DEFAULT (newid()) NOT NULL,
    [SubscriptionId]         UNIQUEIDENTIFIER NOT NULL,
    [NotificationWhatsAppId] UNIQUEIDENTIFIER NOT NULL,
    [WhatsAppId]             VARCHAR (100)    NOT NULL,
    [Status]                 VARCHAR (50)     NOT NULL,
    [ToNumber]               VARCHAR (50)     NULL,
    [ErrorMessage]           VARCHAR (500)    NULL,
    [ErrorCode]              INT              NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              VARCHAR (200)     NULL,
    [ModifiedDate]           DATETIME         NULL,
    [ModifiedBy]             VARCHAR (200)     NULL,
    [DeletedDate]            DATETIME         NULL,
    [IsActive]               BIT              NULL,
    CONSTRAINT [PK_DeliveryReportWhatsApp] PRIMARY KEY CLUSTERED ([Id] ASC)
);







