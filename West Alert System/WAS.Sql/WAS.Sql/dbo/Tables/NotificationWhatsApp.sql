CREATE TABLE [dbo].[NotificationWhatsApp] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_NotificationWhatsApp_Id] DEFAULT (newid()) NOT NULL,
    [Message]        VARCHAR (600)    NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [SentDate]       DATETIME         NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      VARCHAR (200)    NULL,
    [ModifiedDate]   DATETIME         NULL,
    [ModifiedBy]     VARCHAR (200)    NULL,
    [DeletedDate]    DATETIME         NULL,
    [IsActive]       BIT              NULL,
    CONSTRAINT [PK_NotificationWhatsApp] PRIMARY KEY CLUSTERED ([Id] ASC)
);





