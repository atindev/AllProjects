CREATE TABLE [dbo].[VideoCategory] (
    [Id]                  INT            NOT NULL,
    [Category]            NVARCHAR (MAX) NOT NULL,
    [CreatedDate]         DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]           VARCHAR (200)  NULL,
    [ModifiedDate]        DATETIME       NULL,
    [ModifiedBy]          VARCHAR (200)  NULL,
    [DeletedDate]         DATETIME       NULL,
    [IsActive]            BIT            DEFAULT ((1)) NOT NULL,
    [CategoryDescription] NVARCHAR (MAX) NULL,
    [CategoryThumbnail]   NVARCHAR (MAX) NULL,
    [LanguageCode]        VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VideoCategory_GlobalLanguage] FOREIGN KEY ([LanguageCode]) REFERENCES [dbo].[GlobalLanguage] ([Code])
);


GO


GO


GO