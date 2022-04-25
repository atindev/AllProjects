CREATE TABLE [dbo].[Location] (
    [Id]                 INT           NOT NULL,
    [CityID]             INT           NOT NULL,
    [Name]               VARCHAR (100) NOT NULL,
    [Address]            VARCHAR (MAX) NULL,
    [CountryPhoneNumber] VARCHAR (20)  NOT NULL,
    [CreatedDate]        DATETIME      NOT NULL,
    [CreatedBy]          VARCHAR (200)  NULL,
    [ModifiedDate]       DATETIME      NULL,
    [ModifiedBy]         VARCHAR (200)  NULL,
    [DeletedDate]        DATETIME      NULL,
    [IsActive]           BIT           CONSTRAINT [DF_Location_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
ADD SENSITIVITY CLASSIFICATION TO
    [dbo].[Location].[Address]
    WITH (LABEL = 'Confidential', LABEL_ID = '8e514b3a-0ab3-4ab3-85f5-fd1b2367fef9', INFORMATION_TYPE = 'Contact Info', INFORMATION_TYPE_ID = '5c503e21-22c6-81fa-620b-f369b8ec38d1', RANK = MEDIUM);



