CREATE TABLE [dbo].[Event] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_Events_Id] DEFAULT (newid()) NOT NULL,
    [Name]         NVARCHAR (MAX)   NOT NULL,
    [CreatedBy]    VARCHAR (200)    NULL,
    [TypeId]       INT              NOT NULL,
    [ModifiedDate] DATETIME         NOT NULL,
    [CreatedDate]  DATETIME         NOT NULL,
    [ModifiedBy]   VARCHAR (200)    NULL,
    [IsActive]     BIT              NOT NULL,
    [UrgencyId]    INT              NOT NULL,
    [Status]       VARCHAR (100)    NULL,
    [Location]     VARCHAR (100)    NULL,
    [Description]  NVARCHAR (MAX)   NULL,
    [DeletedDate]  DATETIME         NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_EventType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[EventType] ([Id]),
    CONSTRAINT [FK_Event_EventUrgency] FOREIGN KEY ([UrgencyId]) REFERENCES [dbo].[EventUrgency] ([Id])
);



