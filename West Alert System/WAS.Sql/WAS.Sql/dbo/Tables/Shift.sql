CREATE TABLE [dbo].[Shift] (
    [Id]               INT                IDENTITY (1, 1) NOT NULL,
    [ShiftName]        VARCHAR (50)       NOT NULL,
    [ShiftTimingStart] DATETIMEOFFSET (7) NULL,
    [ShiftTimingEnd]   DATETIMEOFFSET (7) NULL,
    [CreatedDate]      DATETIME           CONSTRAINT [DF_Shift_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        VARCHAR (200)       NULL,
    [ModifiedDate]     DATETIME           NULL,
    [ModifiedBy]       VARCHAR (200)       NULL,
    [DeletedDate]      DATETIME           NULL,
    [IsActive]         BIT                CONSTRAINT [DF_Shift_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Shift] PRIMARY KEY CLUSTERED ([Id] ASC)
);



