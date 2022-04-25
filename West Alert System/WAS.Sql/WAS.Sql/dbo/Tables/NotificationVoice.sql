CREATE TABLE [dbo].[NotificationVoice] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Message]        VARCHAR (600)    NOT NULL,
    [RepeatCount]    INT              CONSTRAINT [DF_NotificationVoice_RepeatCount] DEFAULT ((1)) NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [SentDate]       DATETIME         NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      VARCHAR (200)    NULL,
    [ModifiedDate]   DATETIME         NULL,
    [ModifiedBy]     VARCHAR (200)    NULL,
    [DeletedDate]    DATETIME         NULL,
    [IsActive]       BIT              NULL,
    CONSTRAINT [PK_NotificationVoice] PRIMARY KEY CLUSTERED ([Id] ASC)
);





