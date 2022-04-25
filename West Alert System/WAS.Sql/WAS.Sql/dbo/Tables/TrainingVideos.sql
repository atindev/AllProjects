CREATE TABLE [dbo].[TrainingVideos] (
    [Id]               INT            NOT NULL,
    [Name]             NVARCHAR (MAX) NOT NULL,
    [CategoryId]       INT            NOT NULL,
    [URL]              NVARCHAR (MAX) NOT NULL,
    [CreatedDate]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        VARCHAR (200)  NULL,
    [ModifiedDate]     DATETIME       NULL,
    [ModifiedBy]       VARCHAR (200)  NULL,
    [DeletedDate]      DATETIME       NULL,
    [IsActive]         BIT            DEFAULT ((1)) NOT NULL,
    [VideoThumbnail]   NVARCHAR (MAX) NULL,
    [VideoDescription] NVARCHAR (MAX) NULL,
    [VideoDuration]    NVARCHAR (MAX) NULL,
    [LanguageCode]     VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TrainingVideos_GlobalLanguage] FOREIGN KEY ([LanguageCode]) REFERENCES [dbo].[GlobalLanguage] ([Code]),
    CONSTRAINT [FK_TrainingVideos_VideoCategory] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[VideoCategory] ([Id])
);


GO


GO


GO


GO


GO