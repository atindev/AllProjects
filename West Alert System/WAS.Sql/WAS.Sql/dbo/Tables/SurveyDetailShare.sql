CREATE TABLE [dbo].[SurveyDetailShare](
	[Id] [uniqueidentifier] NOT NULL,
	[BroadcastId] [uniqueidentifier] NOT NULL,
	[OfficialMail] [varchar](200) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](200) NULL,
	[DeletedDate] [datetime] NULL,
	[isActive] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SurveyDetailShare]  WITH CHECK ADD  CONSTRAINT [FK_ShareBroadcastedSurvey_SurveyBroadcast] FOREIGN KEY([BroadcastId])
REFERENCES [dbo].[SurveyBroadcast] ([Id])
GO
ALTER TABLE [dbo].[SurveyDetailShare] CHECK CONSTRAINT [FK_ShareBroadcastedSurvey_SurveyBroadcast]
GO
