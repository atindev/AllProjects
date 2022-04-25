CREATE TABLE [dbo].[SubscriptionFeedback] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [SubscriptionId]  UNIQUEIDENTIFIER NOT NULL,
    [FeedbackRating]  VARCHAR (50)     NULL,
    [FeedbackChannel] VARCHAR (50)     NULL,
    [CreatedDate]     DATETIME         CONSTRAINT [DF_SubscriptionFeedback_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       VARCHAR (200)     NULL,
    [ModifiedDate]    DATETIME         NULL,
    [ModifiedBy]      VARCHAR (200)     NULL,
    [DeletedDate]     VARCHAR (50)     NULL,
    [IsActive]        BIT              CONSTRAINT [DF_SubscriptionFeedback_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_SubscriptionFeedback] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Feedback_Subscription] FOREIGN KEY ([SubscriptionId]) REFERENCES [dbo].[Subscription] ([Id])
);

