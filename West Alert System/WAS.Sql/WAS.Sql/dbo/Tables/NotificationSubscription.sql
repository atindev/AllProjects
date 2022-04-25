CREATE TABLE [dbo].[NotificationSubscription]
(
	[Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_NotificationSubscription_Id] DEFAULT (newid()) NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [SubscriptionId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_NotificationSubscription_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      VARCHAR (200)     NULL,
    [ModifiedDate]   DATETIME         NULL,
    [ModifiedBy]     VARCHAR (200)     NULL,
    [DeletedDate]    DATETIME         NULL,
    [IsActive]       BIT              CONSTRAINT [DF_NotificationSubscription_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_NotificationSubscription] PRIMARY KEY CLUSTERED ([Id] ASC)
);
