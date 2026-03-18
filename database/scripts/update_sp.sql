USE db38045
GO

-------------------------------
-- TABLES
-------------------------------

IF OBJECT_ID('dbo.Contacts','U') IS NULL
BEGIN
CREATE TABLE [dbo].[Contacts](
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NULL,
    [PhoneNumber] NVARCHAR(20) NULL,
    [Email] NVARCHAR(150) NULL,
    [Product] NVARCHAR(100) NULL,
    [Message] NVARCHAR(500) NULL,
    [CreatedAt] DATETIME2 DEFAULT GETUTCDATE()
)
END
GO

IF OBJECT_ID('dbo.Customers','U') IS NULL
BEGIN
CREATE TABLE [dbo].[Customers](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL
)
END
GO

IF OBJECT_ID('dbo.login','U') IS NULL
BEGIN
CREATE TABLE [dbo].[login](
    [UserId] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(100),
    [PasswordHash] NVARCHAR(200),
    [Role] NVARCHAR(50),
    [IsActive] BIT
)
END
GO

IF OBJECT_ID('dbo.Products','U') IS NULL
BEGIN
CREATE TABLE [dbo].[Products](
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(MAX),
    [Price] DECIMAL(18,2) NOT NULL,
    [Category] NVARCHAR(150),
    [IsActive] BIT DEFAULT 1,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [UpdatedAt] DATETIME
)
END
GO

IF OBJECT_ID('dbo.ProductImages','U') IS NULL
BEGIN
CREATE TABLE [dbo].[ProductImages](
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [ImageUrl] NVARCHAR(500) NOT NULL,
    [PublicId] NVARCHAR(200),
    [CreatedAt] DATETIME DEFAULT GETDATE()
)
END
GO

IF OBJECT_ID('dbo.ProductVariants','U') IS NULL
BEGIN
CREATE TABLE [dbo].[ProductVariants](
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [Weight] NVARCHAR(50) NOT NULL,
    [OriginalPrice] DECIMAL(18,2) NOT NULL,
    [SellPrice] DECIMAL(18,2) NOT NULL,
    [IsActive] BIT DEFAULT 1,
    [CreatedAt] DATETIME DEFAULT GETDATE()
)
END
GO

IF OBJECT_ID('dbo.Tenants','U') IS NULL
BEGIN
CREATE TABLE [dbo].[Tenants](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100),
    [TenantKey] NVARCHAR(100),
    [FrontendDomain] NVARCHAR(200),
    [ConnectionString] NVARCHAR(MAX)
)
END
GO

IF OBJECT_ID('dbo.Users','U') IS NULL
BEGIN
CREATE TABLE [dbo].[Users](
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(150) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [PhoneNumber] NVARCHAR(20) NOT NULL,
    [Role] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 DEFAULT GETUTCDATE()
)
END
GO

-------------------------------
-- FOREIGN KEYS
-------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_ProductImages_Product')
BEGIN
ALTER TABLE ProductImages
ADD CONSTRAINT FK_ProductImages_Product
FOREIGN KEY (ProductId) REFERENCES Products(Id)
ON DELETE CASCADE
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_ProductVariants_Product')
BEGIN
ALTER TABLE ProductVariants
ADD CONSTRAINT FK_ProductVariants_Product
FOREIGN KEY (ProductId) REFERENCES Products(Id)
ON DELETE CASCADE
END
GO

-------------------------------
-- STORED PROCEDURES
-------------------------------

GO
CREATE OR ALTER PROCEDURE [dbo].[GetAllProductsWithDetails]
AS
BEGIN
    SELECT 
        p.Id,
        p.Name,
        p.Price,
        p.Category,
        p.Description,
        pi.Id AS ImageId,
        pi.ProductId,
        pi.ImageUrl,
        pi.PublicId,
        pi.CreatedAt,
        pv.Id AS VariantId,
        pv.Weight,
        pv.OriginalPrice,
        pv.SellPrice
    FROM Products p
    FULL OUTER JOIN ProductImages pi ON pi.ProductId = p.Id
    FULL OUTER JOIN ProductVariants pv ON pv.ProductId = p.Id
    WHERE p.IsActive = 1
END
GO

GO
CREATE OR ALTER PROCEDURE [dbo].[sp_GetSimpleUsers]
AS
BEGIN
    SELECT *
    FROM products p
    LEFT JOIN productvariants pv ON pv.productid = p.id AND pv.isactive = 1
    LEFT JOIN productimages pi ON pi.productid = p.id
    WHERE p.isactive = 1
END
GO

GO
CREATE OR ALTER PROCEDURE [dbo].[sp_LoginUser]
    @Email NVARCHAR(150)
AS
BEGIN
    SELECT Id, Name, Email, PasswordHash, Role, PhoneNumber
    FROM Users
    WHERE Email = @Email
END
GO

GO
CREATE OR ALTER PROCEDURE [dbo].[sp_LoginUsers]
    @Username NVARCHAR(100),
    @Password NVARCHAR(200)
AS
BEGIN
    SELECT UserId, Username, Role
    FROM login
    WHERE Username = @Username
      AND PasswordHash = @Password
      AND IsActive = 1
END
GO

GO
CREATE OR ALTER PROCEDURE [dbo].[sp_RegisterUser]
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @PasswordHash NVARCHAR(255),
    @PhoneNumber NVARCHAR(20),
    @Role NVARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        RAISERROR('Email already exists',16,1)
        RETURN
    END

    INSERT INTO Users (Name, Email, PasswordHash, PhoneNumber, Role)
    VALUES (@Name, @Email, @PasswordHash, @PhoneNumber, @Role)
END
GO
