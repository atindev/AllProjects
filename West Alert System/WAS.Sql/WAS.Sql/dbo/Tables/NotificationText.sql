CREATE TABLE [dbo].[NotificationText] (
    [Id]				UNIQUEIDENTIFIER CONSTRAINT [DF_NotificationText_Id] DEFAULT (newid()) NOT NULL,
    [Message]			VARCHAR (500)    NOT NULL,
    [NotificationId]	UNIQUEIDENTIFIER NOT NULL,
	[SentDate]			DATETIME         NULL,
	[CreatedDate]		DATETIME         NULL,
	[CreatedBy]			VARCHAR (200)	 NULL,
	[ModifiedDate]		DATETIME         NULL,
	[ModifiedBy]		VARCHAR (200)	 NULL,
	[DeletedDate]		DATETIME         NULL,
	[IsActive]			BIT				 NULL,
    CONSTRAINT [PK_NotificationText] PRIMARY KEY CLUSTERED ([Id] ASC)
);

