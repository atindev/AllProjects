CREATE TABLE [dbo].[SurveyBroadcastFollowup] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [SurveyBroadcastId] UNIQUEIDENTIFIER NOT NULL,
    [Status]            INT              NOT NULL,
    [FollowUpDate]      DATETIME         NOT NULL,
    [SentDate]          DATETIME         NULL,
    [CreatedDate]       DATETIME         NOT NULL,
    [CreatedBy]         VARCHAR (200)     NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        VARCHAR (200)     NULL,
    [DeletedDate]       DATETIME         NULL,
    [IsActive]          BIT              NULL,
    CONSTRAINT [PK_SurveyBroadcastFollowup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyBroadcastFollowup_Survey] FOREIGN KEY ([SurveyBroadcastId]) REFERENCES [dbo].[SurveyBroadcast] ([Id])
);

