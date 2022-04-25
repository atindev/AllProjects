CREATE TABLE [dbo].[SurveyBroadcastText] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_SurveyBroadcastText_Id] DEFAULT (newid()) NOT NULL,
    [SurveyBroadcastId] UNIQUEIDENTIFIER NOT NULL,
    [SentDate]          DATETIME         NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         VARCHAR (200)     NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        VARCHAR (200)     NULL,
    [DeletedDate]       DATETIME         NULL,
    [IsActive]          BIT              NULL,
    CONSTRAINT [PK_SurveyBroadcastText] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyBroadcastText_Survey] FOREIGN KEY ([SurveyBroadcastId]) REFERENCES [dbo].[SurveyBroadcast] ([Id])
);

