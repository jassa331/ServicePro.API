CREATE TABLE [dbo].[ProductVariants] (
    [Id]            UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [ProductId]     UNIQUEIDENTIFIER NOT NULL,
    [Weight]        NVARCHAR (50)    NOT NULL,
    [OriginalPrice] DECIMAL (18, 2)  NOT NULL,
    [SellPrice]     DECIMAL (18, 2)  NOT NULL,
    [IsActive]      BIT              DEFAULT ((1)) NULL,
    [CreatedAt]     DATETIME         DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductVariants_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);

