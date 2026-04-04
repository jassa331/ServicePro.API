CREATE TABLE [dbo].[Contacts] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (100)   NULL,
    [PhoneNumber] NVARCHAR (20)    NULL,
    [Email]       NVARCHAR (150)   NULL,
    [Product]     NVARCHAR (100)   NULL,
    [Message]     NVARCHAR (500)   NULL,
    [CreatedAt]   DATETIME2 (7)    DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

