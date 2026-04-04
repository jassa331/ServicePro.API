CREATE TABLE [dbo].[ProductImages] (
    [Id]        UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [ImageUrl]  NVARCHAR (500)   NOT NULL,
    [PublicId]  NVARCHAR (200)   NULL,
    [CreatedAt] DATETIME         DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductImages_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);

