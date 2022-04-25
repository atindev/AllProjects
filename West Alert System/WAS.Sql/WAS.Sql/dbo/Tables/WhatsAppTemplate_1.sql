CREATE TABLE [dbo].[WhatsAppTemplate] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (100)  NOT NULL,
    [Description]  VARCHAR (1100) NULL,
    [CreatedDate]  DATETIME       NOT NULL,
    [ModifiedDate] DATETIME       NULL,
    [DeletedDate]  DATETIME       NULL,
    [CreatedBy]    VARCHAR (200)  NULL,
    [ModifiedBy]   VARCHAR (200)  NULL,
    [IsActive]     BIT            NULL,
    CONSTRAINT [PK_WhatsAppTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

