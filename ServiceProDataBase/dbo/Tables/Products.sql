CREATE TABLE [dbo].[Products] (
    [Id]          UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]        NVARCHAR (200)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [Price]       DECIMAL (18, 2)  NOT NULL,
    [Category]    NVARCHAR (150)   NULL,
    [IsActive]    BIT              DEFAULT ((1)) NULL,
    [CreatedAt]   DATETIME         DEFAULT (getdate()) NULL,
    [UpdatedAt]   DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

