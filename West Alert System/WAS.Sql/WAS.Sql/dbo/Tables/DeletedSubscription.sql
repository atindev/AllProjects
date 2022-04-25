CREATE TABLE [dbo].[DeletedSubscription] (
    [Id]                       UNIQUEIDENTIFIER                                  NOT NULL,
    [LocationId]               INT                                               NOT NULL,
    [FirstName]                VARCHAR (50)                                      NOT NULL,
    [LastName]                 VARCHAR (50)                                      NOT NULL,
    [OfficialEmail]            VARCHAR (200)                                     NULL,
    [PersonalEmail]            VARCHAR (200) MASKED WITH (FUNCTION = 'email()')  NULL,
    [OfficeMobile]             VARCHAR (20)                                      NULL,
    [PersonalMobile]           VARCHAR (20) MASKED WITH (FUNCTION = 'default()') NULL,
    [OfficePhone]              VARCHAR (20)                                      NULL,
    [HomePhone]                VARCHAR (20)                                      NULL,
    [Time]                     VARCHAR (10)                                      NULL,
    [IsOfficialEmail]          BIT                                               NOT NULL,
    [IsPersonalEmail]          BIT                                               NOT NULL,
    [IsVoiceOfficeMobile]      BIT                                               NOT NULL,
    [IsVoicePersonalMobile]    BIT                                               NOT NULL,
    [IsVoiceOfficePhone]       BIT                                               NOT NULL,
    [IsVoiceHomePhone]         BIT                                               NOT NULL,
    [IsTextOfficeMobile]       BIT                                               NOT NULL,
    [IsTextPersonalMobile]     BIT                                               NOT NULL,
    [IsWhatsAppOfficeMobile]   BIT                                               NOT NULL,
    [IsWhatsAppPersonalMobile] BIT                                               NOT NULL,
    [IsTeams]                  BIT                                               NOT NULL,
    [Consent]                  BIT                                               NULL,
    [EmployeeId]               VARCHAR (50)                                      NULL,
    [Upn]                      VARCHAR (100)                                     NOT NULL,
    [City]                     VARCHAR (100)                                     NULL,
    [State]                    VARCHAR (100)                                     NULL,
    [Country]                  VARCHAR (100)                                     NULL,
    [PostalCode]               INT                                               NULL,
    [DepartmentId]             INT                                               NULL,
    [JobTitle]                 VARCHAR (100)                                     NULL,
    [EmployeeType]             VARCHAR (100)                                     NULL,
    [EmployeeGroup]            VARCHAR (100)                                     NULL,
    [CostCenter]               VARCHAR (50)                                      NULL,
    [CreatedDate]              DATETIME                                          NOT NULL,
    [CreatedBy]                VARCHAR (200)                                      NULL,
    [SubscriptionMode]         VARCHAR (20)                                      NULL,
    [ModifiedDate]             DATETIME                                          NULL,
    [ModifiedBy]               VARCHAR (200)                                      NULL,
    [DeletedDate]              DATETIME                                          NULL,
    [IsActive]                 BIT                                               NOT NULL,
    [ShiftId]                  INT                                               NULL,
    CONSTRAINT [PK_DeletedSubscription] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[FirstName]
    WITH (LABEL = 'Confidential - GDPR', LABEL_ID = '98e83899-90c7-46f1-87f8-2de308f7e06c', INFORMATION_TYPE = 'Name', INFORMATION_TYPE_ID = '57845286-7598-22f5-9659-15b24aeb125e', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[LastName]
    WITH (LABEL = 'Confidential - GDPR', LABEL_ID = '98e83899-90c7-46f1-87f8-2de308f7e06c', INFORMATION_TYPE = 'Name', INFORMATION_TYPE_ID = '57845286-7598-22f5-9659-15b24aeb125e', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[OfficialEmail]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[PersonalEmail]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[OfficeMobile]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[PersonalMobile]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[OfficePhone]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[HomePhone]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[City]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[DeletedSubscription].[PostalCode]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);
 
GO