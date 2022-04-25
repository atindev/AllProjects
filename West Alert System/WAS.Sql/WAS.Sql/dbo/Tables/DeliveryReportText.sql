CREATE TABLE [dbo].[DeliveryReportText] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_DeliveryReportText_Id] DEFAULT (newid()) NOT NULL,
    [SubscriptionId]     UNIQUEIDENTIFIER NOT NULL,
    [NotificationTextId] UNIQUEIDENTIFIER NOT NULL,
    [SmsId]              VARCHAR (100)    NOT NULL,
    [Status]             VARCHAR (50)     NOT NULL,
    [ErrorMessage]       VARCHAR (500)    NULL,
    [ErrorCode]          INT              NULL,
    [ToNumber]           VARCHAR (50)     NULL,
    [CreatedDate]        DATETIME         NULL,
    [CreatedBy]          VARCHAR (200)     NULL,
    [ModifiedDate]       DATETIME         NULL,
    [ModifiedBy]         VARCHAR (200)     NULL,
    [DeletedDate]        DATETIME         NULL,
    [IsActive]           BIT              NULL,
    CONSTRAINT [PK_DeliveryReportText] PRIMARY KEY CLUSTERED ([Id] ASC)
);






