CREATE TABLE [dbo].[EventType] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [CreatedDate]  DATETIME     CONSTRAINT [DF_EventType_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedDate] DATETIME     NULL,
    [DeletedDate]  DATETIME     NULL,
    [CreatedBy]    VARCHAR (200) NULL,
    [ModifiedBy]   VARCHAR (200) NULL,
    [IsActive]     BIT          CONSTRAINT [DF_EventType_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_EventsType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

