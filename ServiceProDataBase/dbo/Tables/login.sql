CREATE TABLE [dbo].[login] (
    [UserId]       INT            IDENTITY (1, 1) NOT NULL,
    [Username]     NVARCHAR (100) NULL,
    [PasswordHash] NVARCHAR (200) NULL,
    [Role]         NVARCHAR (50)  NULL,
    [IsActive]     BIT            NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

