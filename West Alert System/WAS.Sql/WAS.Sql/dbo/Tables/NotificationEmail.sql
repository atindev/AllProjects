CREATE TABLE [dbo].[NotificationEmail] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_NotificationEmail_Id] DEFAULT (newid()) NOT NULL,
    [Subject]        VARCHAR (100)    NOT NULL,
    [Body]           VARCHAR (MAX)    NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [SentDate]       DATETIME         NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      VARCHAR (200)    NULL,
    [ModifiedDate]   DATETIME         NULL,
    [ModifiedBy]     VARCHAR (200)    NULL,
    [DeletedDate]    DATETIME         NULL,
    [IsActive]       BIT              NULL,
    CONSTRAINT [PK_NotificationEmail] PRIMARY KEY CLUSTERED ([Id] ASC)
);



