CREATE TABLE [dbo].[TemplateCategory] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [CreatedDate]  DATETIME     CONSTRAINT [DF_TemplateCategory_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedDate] DATETIME     NULL,
    [DeletedDate]  DATETIME     NULL,
    [CreatedBy]    VARCHAR (200) NULL,
    [ModifiedBy]   VARCHAR (200) NULL,
    [IsActive]     BIT          CONSTRAINT [DF_TemplateCategory_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TemplateCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

