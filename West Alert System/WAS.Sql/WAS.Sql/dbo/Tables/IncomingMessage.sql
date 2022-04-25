CREATE TABLE [dbo].[IncomingMessage] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_IncomingMessage_Id] DEFAULT (newid()) NOT NULL,
    [SubscriberEmail] VARCHAR (200)    NULL,
    [NotificationId]  UNIQUEIDENTIFIER NULL,
    [Message]         VARCHAR (MAX)    NULL,
    [FromPhone]       VARCHAR (20)     NULL,
    [IsText]          BIT              CONSTRAINT [DF_IncomingMessage_IsText] DEFAULT ((0)) NOT NULL,
    [IsVoice]         BIT              CONSTRAINT [DF_IncomingMessage_IsVoice] DEFAULT ((0)) NOT NULL,
    [IsWhatsApp]      BIT              CONSTRAINT [DF_IncomingMessage_IsWhatsApp] DEFAULT ((0)) NOT NULL,
    [IsEmail]         BIT              CONSTRAINT [DF_IncomingMessage_IsEmail] DEFAULT ((0)) NOT NULL,
    [EmailSubject]    VARCHAR (MAX)    NULL,
    [FromEmail]       VARCHAR (100)    NULL,
    [TwilioVoiceMailUrl] VARCHAR(MAX)  NULL,
    [CreatedDate]     DATETIME         CONSTRAINT [DF__IncomingM__Creat__08012052] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       VARCHAR (200)     NULL,
    [ModifiedDate]    DATETIME         NULL,
    [ModifiedBy]      VARCHAR (200)     NULL,
    [DeletedDate]     DATETIME         NULL,
    [IsActive]        BIT              CONSTRAINT [DF_IncomingMessage_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_IncomingMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[IncomingMessage].[SubscriberEmail]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[IncomingMessage].[FromPhone]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);



