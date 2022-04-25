CREATE TABLE [dbo].[City] (
    [Id]      INT          IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (50) NOT NULL,
    [StateID] INT          NOT NULL,
    [CreatedDate] datetime default getDate() not null, 
    [CreatedBy] varchar(200) null,
    [ModifiedDate] datetime null,
    [ModifiedBy]varchar(200)  null,
    [DeletedDate] datetime  null,
    [IsActive] bit default 1 not null,
    CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_City_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([Id])
);

