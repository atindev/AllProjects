CREATE TABLE [dbo].[SurveyBroadcastADUser](
	[Id]									UNIQUEIDENTIFIER NOT NULL DEFAULT (newid()),
	[SurveyBroadcastId]						UNIQUEIDENTIFIER NOT NULL,
	[FirstName]								VARCHAR(100)	 NULL,
	[LastName]								VARCHAR(100)	 NULL,
	[EmailId]								VARCHAR(200)	 NOT NULL,
	[DepartmentId]							INT				 NULL,
	[LocationId]							INT				 NULL,
	[SurveyBroadcastDistributionGroupId]	UNIQUEIDENTIFIER NULL,
	[CreatedDate]							DATETIME		 NOT NULL DEFAULT (getdate()),
	[CreatedBy]								VARCHAR(200)	 NULL,
	[ModifiedDate]							DATETIME		 NULL,
	[ModifiedBy]							VARCHAR(200)	 NULL,
	[DeletedDate]							DATETIME		 NULL,
	[IsActive]								BIT				 NULL DEFAULT ((1)),
 CONSTRAINT [PK_SurveyBroadcastADUser] PRIMARY KEY CLUSTERED ([Id] ASC),
 CONSTRAINT [FK_SurveyBroadcastADUser_SurveyBroadcast] FOREIGN KEY ([SurveyBroadcastId]) REFERENCES [dbo].[SurveyBroadcast] ([Id]),
 CONSTRAINT [FK_SurveyBroadcastADUser_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]),
 CONSTRAINT [FK_SurveyBroadcastADUser_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Department] ([Id]),
 CONSTRAINT [FK_SurveyBroadcastADUser_SurveyBroadcastDistributionGroup] FOREIGN KEY ([SurveyBroadcastDistributionGroupId]) REFERENCES [dbo].[SurveyBroadcastDistributionGroup] ([Id])
 );

