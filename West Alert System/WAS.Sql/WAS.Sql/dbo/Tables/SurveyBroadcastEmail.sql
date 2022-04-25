CREATE TABLE [dbo].[SurveyBroadcastEmail] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_SurveyBroadcastEmail_Id] DEFAULT (newid()) NOT NULL,
    [SurveyBroadcastId] UNIQUEIDENTIFIER NOT NULL,
    [SentDate]          DATETIME         NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         VARCHAR (200)     NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        VARCHAR (200)     NULL,
    [DeletedDate]       DATETIME         NULL,
    [IsActive]          BIT              NULL,
    CONSTRAINT [PK_SurveyBroadcastEmail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyBroadcastEmail_Survey] FOREIGN KEY ([SurveyBroadcastId]) REFERENCES [dbo].[SurveyBroadcast] ([Id])
);

