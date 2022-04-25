CREATE TABLE [dbo].[State] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [CountryID] INT          NOT NULL,
    [CreatedDate] datetime default getDate() not null,
    [CreatedBy] varchar(200) null,
    [ModifiedDate] datetime null,
    [ModifiedBy]varchar(200)  null,
    [DeletedDate] datetime  null,
    [IsActive] bit default 1 not null,
    CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_State_Country] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([Id])
);

