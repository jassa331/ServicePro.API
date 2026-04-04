CREATE TABLE [dbo].[Users] (
    [Id]           UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]         NVARCHAR (100)   NOT NULL,
    [Email]        NVARCHAR (150)   NOT NULL,
    [PasswordHash] NVARCHAR (255)   NOT NULL,
    [PhoneNumber]  NVARCHAR (20)    NOT NULL,
    [Role]         NVARCHAR (50)    NOT NULL,
    [CreatedAt]    DATETIME2 (7)    DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

