CREATE TABLE [dbo].[Template] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_Template_Id] DEFAULT (newid()) NOT NULL,
    [Name]         VARCHAR (50)     NOT NULL,
    [Description]  VARCHAR (500)    NULL,
    [Path]         VARCHAR (MAX)    NOT NULL,
    [CreatedDate]  DATETIME         NOT NULL,
    [CreatedBy]    VARCHAR (200)     NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   VARCHAR (200)     NULL,
    [DeletedDate]  DATETIME         NULL,
    [CategoryId]   INT              NOT NULL,
    [IsActive]     BIT              NULL,
    CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Template_TemplateCategory] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[TemplateCategory] ([Id])
);





