CREATE TABLE [dbo].[SurveyBroadcastSubscription] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_SurveyBroadcastSubscription_Id] DEFAULT (newid()) NOT NULL,
    [SurveyBroadcastId] UNIQUEIDENTIFIER NOT NULL,
    [SubscriptionId]    UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]       DATETIME         CONSTRAINT [DF_SurveyBroadcastSubscription_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         VARCHAR (200)     NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        VARCHAR (200)     NULL,
    [DeletedDate]       DATETIME         NULL,
    [IsActive]          BIT              CONSTRAINT [DF_SurveyBroadcastSubscription_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_SurveyBroadcastSubscription] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyBroadcastSubscription_Subscription] FOREIGN KEY ([SubscriptionId]) REFERENCES [dbo].[Subscription] ([Id]),
    CONSTRAINT [FK_SurveyBroadcastSubscription_Survey] FOREIGN KEY ([SurveyBroadcastId]) REFERENCES [dbo].[SurveyBroadcast] ([Id])
);

