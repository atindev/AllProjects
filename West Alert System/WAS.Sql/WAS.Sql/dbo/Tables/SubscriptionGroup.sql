CREATE TABLE [dbo].[SubscriptionGroup] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_SubscriptionGroup_Id] DEFAULT (newid()) NOT NULL,
    [SubscriptionId] UNIQUEIDENTIFIER NOT NULL,
    [GroupId]        INT              NOT NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      VARCHAR(200)    NULL,
    [ModifiedDate]   DATETIME         NULL,
    [ModifiedBy]     VARCHAR(200)    NULL,
    [DeletedDate]    DATETIME         NULL,
    [IsActive]       BIT              CONSTRAINT [DF_SubscriptionGroup_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_SubscriptionGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

