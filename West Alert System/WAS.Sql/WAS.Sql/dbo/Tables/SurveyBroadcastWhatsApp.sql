CREATE TABLE [dbo].[SurveyBroadcastWhatsApp] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_SurveyBroadcastWhatsApp_Id] DEFAULT (newid()) NOT NULL,
    [SurveyBroadcastId] UNIQUEIDENTIFIER NOT NULL,
    [SentDate]          DATETIME         NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         VARCHAR (200)     NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        VARCHAR (200)     NULL,
    [DeletedDate]       DATETIME         NULL,
    [IsActive]          BIT              NULL,
    CONSTRAINT [PK_SurveyBroadcastWhatsApp] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyBroadcastWhatsApp_Survey] FOREIGN KEY ([SurveyBroadcastId]) REFERENCES [dbo].[SurveyBroadcast] ([Id])
);

