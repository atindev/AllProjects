CREATE TABLE [dbo].[SurveyBroadcastGroup] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_SurveyBroadcastGroup_Id] DEFAULT (newid()) NOT NULL,
    [SurveyBroadcastId] UNIQUEIDENTIFIER NOT NULL,
    [GroupId]           INT              NOT NULL,
    [CreatedDate]       DATETIME         CONSTRAINT [DF_SurveyBroadcastGroup_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         VARCHAR (200)     NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        VARCHAR (200)     NULL,
    [DeletedDate]       DATETIME         NULL,
    [IsActive]          BIT              CONSTRAINT [DF_SurveyBroadcastGroup_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_SurveyBroadcastGroup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyBroadcastGroup_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]),
    CONSTRAINT [FK_SurveyBroadcastGroup_Survey] FOREIGN KEY ([SurveyBroadcastId]) REFERENCES [dbo].[SurveyBroadcast] ([Id])
);

