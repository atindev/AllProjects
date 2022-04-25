CREATE TABLE [dbo].[Country] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [RegionID] INT          NULL,
    [CreatedDate] datetime default getDate() not null,
    [CreatedBy] varchar(200) null,
    [ModifiedDate] datetime null,
    [ModifiedBy]varchar(200)  null,
    [DeletedDate] datetime  null,
    [IsActive] bit default 1 not null,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Country_Region] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[Region] ([Id])
);

