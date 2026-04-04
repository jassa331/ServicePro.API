CREATE TABLE [dbo].[Tenants] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (100) NULL,
    [TenantKey]        NVARCHAR (100) NULL,
    [FrontendDomain]   NVARCHAR (200) NULL,
    [ConnectionString] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

