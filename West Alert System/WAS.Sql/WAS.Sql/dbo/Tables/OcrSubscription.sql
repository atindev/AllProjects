CREATE TABLE [dbo].[OcrSubscription] (
    [Id]                                 UNIQUEIDENTIFIER CONSTRAINT [DF_OcrSubscription_Id] DEFAULT (newid()) NOT NULL,
    [FirstName]                          VARCHAR (100)    NULL,
    [FirstNameConfidence]                FLOAT (53)       NULL,
    [LastName]                           VARCHAR (100)    NULL,
    [LastNameConfidence]                 FLOAT (53)       NULL,
    [EmployeeId]                         VARCHAR (50)     NULL,
    [EmployeeIdConfidence]               FLOAT (53)       NULL,
    [UserId]                             VARCHAR (200)    NULL,
    [[UserIdConfidence]                  FLOAT (53)       NULL,
    [OfficialEmail]                      VARCHAR (200)    NULL,
    [OfficialEmailConfidence]            FLOAT (53)       NULL,
    [PersonalEmail]                      VARCHAR (200)    NULL,
    [PersonalEmailConfidence]            FLOAT (53)       NULL,
    [HomePhone]                          VARCHAR (20)     NULL,
    [HomePhoneConfidence]                FLOAT (53)       NULL,
    [PersonalMobile]                     VARCHAR (20)     NULL,
    [PersonalMobileConfidence]           FLOAT (53)       NULL,
    [IsOfficialEmail]                    BIT              CONSTRAINT [DF_OcrSubscription_IsOfficialEmail] DEFAULT ((1)) NOT NULL,
    [IsPersonalEmail]                    BIT              CONSTRAINT [DF_OcrSubscription_IsPersonalEmail] DEFAULT ((0)) NOT NULL,
    [IsTextPersonalMobile]               BIT              CONSTRAINT [DF_OcrSubscription_IsTextPersonalMobile] DEFAULT ((0)) NOT NULL,
    [IsTextPersonalMobileConfidence]     FLOAT (53)       NULL,
    [IsVoicePersonalMobile]              BIT              CONSTRAINT [DF_OcrSubscription_IsVoicePersonalMobile] DEFAULT ((0)) NOT NULL,
    [IsVoicePersonalMobileConfidence]    FLOAT (53)       NULL,
    [IsWhatsAppPersonalMobile]           BIT              CONSTRAINT [DF_OcrSubscription_IsWhatsAppPersonalMobile] DEFAULT ((0)) NOT NULL,
    [IsWhatsAppPersonalMobileConfidence] FLOAT (53)       NULL,
    [IsVoiceHomePhone]                   BIT              CONSTRAINT [DF_OcrSubscription_IsVoiceHomePhone] DEFAULT ((0)) NOT NULL,
    [Consent]                            BIT              CONSTRAINT [DF_OcrSubscription_Consent] DEFAULT ((0)) NOT NULL,
    [ConsentConfidence]                  FLOAT (53)       NULL,
    [FileName]                           VARCHAR (MAX)    NULL,
    [FilePath]                           VARCHAR (MAX)    NULL,
    [LocationId]                         INT              CONSTRAINT [DF_OcrSubscription_LocationId] DEFAULT ((0)) NOT NULL,
    [CreatedDate]                        DATETIME         NOT NULL,
    [CreatedBy]                          VARCHAR (200)     NULL,
    [ModifiedDate]                       DATETIME         NULL,
    [ModifiedBy]                         VARCHAR (200)     NULL,
    [DeletedDate]                        DATETIME         NULL,
    [IsActive]                           BIT              CONSTRAINT [DF_OcrSubscription_IsActive] DEFAULT ((1)) NOT NULL
);





