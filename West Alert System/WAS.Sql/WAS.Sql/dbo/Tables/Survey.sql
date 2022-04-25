CREATE TABLE [dbo].[Survey] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_Survey_Id] DEFAULT (newid()) NOT NULL,
    [Subject]           VARCHAR (100)    NOT NULL,
    [Description]       VARCHAR (500)    NULL,
    [NumberofQuestions] INT              NULL,
    [Path]              VARCHAR (MAX)    NOT NULL,
    [CreatedDate]       DATETIME         NOT NULL,
    [CreatedBy]         VARCHAR (200)     NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        VARCHAR (200)     NULL,
    [DeletedDate]       DATETIME         NULL,
    [IsActive]          BIT              NULL,
    CONSTRAINT [PK_Survey] PRIMARY KEY CLUSTERED ([Id] ASC)
);



