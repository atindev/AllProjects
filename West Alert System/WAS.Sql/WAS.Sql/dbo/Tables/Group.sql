CREATE TABLE [dbo].[Group] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50)  NOT NULL,
    [LocationID]       INT           NOT NULL,
    [CreatedDate]      DATETIME      NULL,
    [CreatedBy]        VARCHAR(200) NULL,
    [ModifiedDate]     DATETIME      NULL,
    [ModifiedBy]       VARCHAR(200) NULL,
    [DeletedDate]      DATETIME      NULL,
    [IsActive]         BIT           NULL,
    [Description]      VARCHAR (500) NULL,
    [IsPrivate]        BIT           NULL,
    [IsAccessToAdmins] BIT           NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([Id] ASC)
);







