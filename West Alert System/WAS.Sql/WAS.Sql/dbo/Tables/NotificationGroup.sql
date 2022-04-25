CREATE TABLE [dbo].[NotificationGroup] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_NotificationGroup_Id] DEFAULT (newid()) NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [GroupId]        INT              NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_NotificationGroup_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      VARCHAR (200)     NULL,
    [ModifiedDate]   DATETIME         NULL,
    [ModifiedBy]     VARCHAR (200)     NULL,
    [DeletedDate]    DATETIME         NULL,
    [IsActive]       BIT              CONSTRAINT [DF_NotificationGroup_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_NotificationGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);



