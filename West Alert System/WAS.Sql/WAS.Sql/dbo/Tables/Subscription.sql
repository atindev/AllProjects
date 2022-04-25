CREATE TABLE [dbo].[Subscription] (
    [Id]                       UNIQUEIDENTIFIER                                  DEFAULT (newid()) NOT NULL,
    [LocationId]               INT                                               NOT NULL,
    [FirstName]                VARCHAR (100)                                      NOT NULL,
    [LastName]                 VARCHAR (100)                                      NOT NULL,
    [OfficialEmail]            VARCHAR (200)                                     NULL,
    [PersonalEmail]            VARCHAR (200) MASKED WITH (FUNCTION = 'email()')  NULL,
    [OfficeMobile]             VARCHAR (20)                                      NULL,
    [PersonalMobile]           VARCHAR (20) MASKED WITH (FUNCTION = 'default()') NULL,
    [OfficePhone]              VARCHAR (20)                                      NULL,
    [HomePhone]                VARCHAR (20) MASKED WITH (FUNCTION = 'default()') NULL,
    [Time]                     VARCHAR (10)                                      NULL,
    [IsOfficialEmail]          BIT                                               CONSTRAINT [DF_Subscription_IsOfficialEmail] DEFAULT ((0)) NOT NULL,
    [IsPersonalEmail]          BIT                                               CONSTRAINT [DF_Subscription_IsPersonalEmail] DEFAULT ((0)) NOT NULL,
    [IsVoiceOfficeMobile]      BIT                                               CONSTRAINT [DF_Subscription_IsOfficeMobile] DEFAULT ((0)) NOT NULL,
    [IsVoicePersonalMobile]    BIT                                               CONSTRAINT [DF_Subscription_IsPersonalMobile] DEFAULT ((0)) NOT NULL,
    [IsVoiceOfficePhone]       BIT                                               CONSTRAINT [DF_Subscription_IsOfficePhone] DEFAULT ((0)) NOT NULL,
    [IsVoiceHomePhone]         BIT                                               CONSTRAINT [DF_Subscription_IsHomePhone] DEFAULT ((0)) NOT NULL,
    [IsTextOfficeMobile]       BIT                                               CONSTRAINT [DF_Subscription_IsTextOfficeMobile] DEFAULT ((0)) NOT NULL,
    [IsTextPersonalMobile]     BIT                                               CONSTRAINT [DF_Subscription_IsTextPersonalMobile] DEFAULT ((0)) NOT NULL,
    [IsWhatsAppOfficeMobile]   BIT                                               CONSTRAINT [DF_Subscription_IsWhatsAppOfficeMobile] DEFAULT ((0)) NOT NULL,
    [IsWhatsAppPersonalMobile] BIT                                               CONSTRAINT [DF_Subscription_IsWatsAppPersonalMobile] DEFAULT ((0)) NOT NULL,
    [IsTeams]                  BIT                                               CONSTRAINT [DF_Subscription_IsTeams] DEFAULT ((0)) NOT NULL,
    [Consent]                  BIT                                               NULL,
    [EmployeeId]               VARCHAR (50)                                      NULL,
    [Upn]                      VARCHAR (100)                                     NOT NULL,
    [City]                     VARCHAR (100)                                     NULL,
    [State]                    VARCHAR (100)                                     NULL,
    [Country]                  VARCHAR (100)                                     NULL,
    [PostalCode]               INT                                               NULL,
    [DepartmentId]               INT                                               NULL,
    [JobTitle]                 VARCHAR (100)                                     NULL,
    [EmployeeType]             VARCHAR (100)                                     NULL,
    [EmployeeGroup]            VARCHAR (100)                                     NULL,
    [CostCenter]               VARCHAR (50)                                      NULL,
    [SubscriptionMode]         VARCHAR (20)                                      NULL,
    [ShiftId]                  INT                                               NULL,
    [UpdatedOn]                DATETIME                                          NULL,
    [UpdatedTimeZone]          VARCHAR (50)                                      NULL,
    [PreferredChannel]          VARCHAR (50)                                     NULL,
    [PreferredLanguage]         VARCHAR(50)                                      NULL, 
    [Role]                      VARCHAR(50)                                      NULL, 
    [CreatedDate]              DATETIME                                          NOT NULL,
    [CreatedBy]                VARCHAR (200)                                      NULL,
    [ModifiedDate]             DATETIME                                          NULL,
    [ModifiedBy]               VARCHAR (200)                                      NULL,
    [DeletedDate]              DATETIME                                          NULL,
    [IsActive]                 BIT                                               CONSTRAINT [DF_Subscription_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[FirstName]
    WITH (LABEL = 'Confidential - GDPR', LABEL_ID = '98e83899-90c7-46f1-87f8-2de308f7e06c', INFORMATION_TYPE = 'Name', INFORMATION_TYPE_ID = '57845286-7598-22f5-9659-15b24aeb125e', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[LastName]
    WITH (LABEL = 'Confidential - GDPR', LABEL_ID = '98e83899-90c7-46f1-87f8-2de308f7e06c', INFORMATION_TYPE = 'Name', INFORMATION_TYPE_ID = '57845286-7598-22f5-9659-15b24aeb125e', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[OfficialEmail]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[PersonalEmail]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[OfficeMobile]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[PersonalMobile]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[OfficePhone]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[HomePhone]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[City]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);


GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Subscription].[PostalCode]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);











