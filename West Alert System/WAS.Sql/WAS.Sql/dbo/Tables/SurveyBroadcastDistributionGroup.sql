CREATE TABLE [dbo].[SurveyBroadcastDistributionGroup] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL DEFAULT (newid()),
    [SurveyBroadcastId]     UNIQUEIDENTIFIER NOT NULL,
    [DistributionGroup]     VARCHAR(200)     NOT NULL, 
    [DistributionGroupId]   VARCHAR(100)     NOT NULL, 
    [DistributionGroupName] VARCHAR(100)     NULL,
    [CreatedDate]           DATETIME         NOT NULL DEFAULT (getdate()),
    [CreatedBy]             VARCHAR (200)    NULL,
    [ModifiedDate]          DATETIME         NULL,
    [ModifiedBy]            VARCHAR (200)    NULL,
    [DeletedDate]           DATETIME         NULL,
    [IsActive]              BIT              NOT NULL DEFAULT ((1)),

    CONSTRAINT [PK_SurveyBroadDistributionGroup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyBroadcastDistributionGroup_SurveyBroadcast] FOREIGN KEY ([SurveyBroadcastId]) REFERENCES [dbo].[SurveyBroadcast] ([Id])
);

