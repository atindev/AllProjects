CREATE TABLE [dbo].[NotificationEmailAttachment] (
    [Id]                  UNIQUEIDENTIFIER CONSTRAINT [DF_NotificationEmailAttachment_Id] DEFAULT (newid()) NOT NULL,
    [NotificationEmailId] UNIQUEIDENTIFIER NOT NULL,
    [FileName]            VARCHAR (100)     NOT NULL,
    [Attachment]          VARCHAR (MAX)    NOT NULL,
    [ContentType]         VARCHAR (500)     NOT NULL,
    [CreatedDate]		  DATETIME         NULL,
	[CreatedBy]			  VARCHAR (200)	   NULL,
	[ModifiedDate]		  DATETIME         NULL,
	[ModifiedBy]		  VARCHAR (200)	   NULL,
	[DeletedDate]		  DATETIME         NULL,
	[IsActive]			  BIT			   NULL,
    CONSTRAINT [PK_NotificationEmailAttachment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NotificationEmailAttachment_NotificationEmail] FOREIGN KEY ([NotificationEmailId]) REFERENCES [dbo].[NotificationEmail] ([Id])
);

