CREATE TABLE [dbo].[BlockedUser] (
    [Id]            UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [OfficialEmail] VARCHAR (200)    NULL,
    [EmployeeId]    VARCHAR (50)     NULL,
    [AttemptFrom]   VARCHAR (20)     NULL,
    [CreatedDate]   DATETIME         CONSTRAINT [DF_BlockedUser_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]  DATETIME         NULL,
    [DeletedDate]   DATETIME         NULL,
    [CreatedBy]     VARCHAR (200)     NULL,
    [ModifiedBy]    VARCHAR (200)     NULL,
    [IsActive]      BIT              CONSTRAINT [DF_BlockedUser_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_BlockedUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);

