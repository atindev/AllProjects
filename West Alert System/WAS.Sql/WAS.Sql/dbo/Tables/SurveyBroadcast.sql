CREATE TABLE [dbo].[SurveyBroadcast] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [SurveyId]            UNIQUEIDENTIFIER NOT NULL,
    [StartTime]           DATETIME         NULL,
    [EndTime]             DATETIME         NULL,
    [TimeZone]            VARCHAR (500)    NULL,
    [TimeZoneOffset]      INT              CONSTRAINT [DF_SurveyBroadcast_TimeZoneOffset] DEFAULT ((0)) NOT NULL,
    [IsText]              BIT              CONSTRAINT [DF_SurveyBroadcast_IsText] DEFAULT ((0)) NOT NULL,
    [IsEmail]             BIT              CONSTRAINT [DF_SurveyBroadcast_IsEmail] DEFAULT ((0)) NOT NULL,
    [IsTeams]             BIT              CONSTRAINT [DF_SurveyBroadcast_IsTeams] DEFAULT ((0)) NOT NULL,
    [IsWhatsApp]          BIT              CONSTRAINT [DF_SurveyBroadcast_IsWhatsApp] DEFAULT ((0)) NOT NULL,
    [Status]              INT              NOT NULL,
    [Subject]             VARCHAR (100)    NULL,
    [Description]         VARCHAR (500)    NULL,
    [NumberofQuestions]   INT              NULL,
    [Path]                VARCHAR (MAX)    NULL,
    [IsWizard]            BIT              CONSTRAINT [DF_SurveyBroadcast_IsWizard] DEFAULT ((0)) NULL,
    [SentDate]            DATETIME         NULL,
    [BroadcastedTimeZone] VARCHAR (500)    NULL,
    [TotalAudienceCount] INT NOT NULL DEFAULT 0, 
    [CreatedDate]         DATETIME         NOT NULL,
    [CreatedBy]           VARCHAR (200)    NULL,
    [ModifiedDate]        DATETIME         NULL,
    [ModifiedBy]          VARCHAR (200)    NULL,
    [DeletedDate]         DATETIME         NULL,
    [IsActive]            BIT              NULL,
    CONSTRAINT [PK_SurveyBroadcast] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyBroadcast_Survey] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([Id])
);







