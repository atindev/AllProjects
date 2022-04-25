CREATE TABLE [dbo].[Department]
(
	[Id]           INT           NOT NULL,
    [Name]         VARCHAR (100) NOT NULL,
    [CreatedDate]  DATETIME      NOT NULL,
    [CreatedBy]    VARCHAR (200)  NULL,
    [ModifiedDate] DATETIME      NULL,
    [ModifiedBy]   VARCHAR (200)  NULL,
    [DeletedDate]  DATETIME      NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Department_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([Id] ASC)
)
