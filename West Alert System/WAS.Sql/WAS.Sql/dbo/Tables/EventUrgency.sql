CREATE TABLE [dbo].[EventUrgency] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [CreatedDate]  DATETIME     CONSTRAINT [DF_EventUrgency_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedDate] DATETIME     NULL,
    [DeletedDate]  DATETIME     NULL,
    [CreatedBy]    VARCHAR (200) NULL,
    [ModifiedBy]   VARCHAR (200) NULL,
    [IsActive]     BIT          CONSTRAINT [DF_EventUrgency_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_EventsUrgency] PRIMARY KEY CLUSTERED ([Id] ASC)
);

