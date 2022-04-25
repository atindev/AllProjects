CREATE TABLE [dbo].[NotificationType] (
    [Id]   INT          NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    [CreatedDate]		DATETIME DEFAULT getDate() NOT NULL,
	[CreatedBy]			VARCHAR (200)	 NULL,
	[ModifiedDate]		DATETIME         NULL,
	[ModifiedBy]		VARCHAR (200)	 NULL,
	[DeletedDate]		DATETIME         NULL,
	[IsActive]          BIT           CONSTRAINT [DF_NotificationType_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_NotificationType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

