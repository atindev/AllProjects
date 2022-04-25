CREATE TABLE [dbo].[Language] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (500) NOT NULL,
    [Code]         VARCHAR (50)  NOT NULL,
    [CreatedDate]  DATETIME      NOT NULL,
    [ModifiedDate] DATETIME      NULL,
    [DeletedDate]  DATETIME      NULL,
    [CreatedBy]    VARCHAR (200)  NULL,
    [ModifiedBy]   VARCHAR (200)  NULL,
    [IsActive]     BIT           NOT NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id] ASC)
);





