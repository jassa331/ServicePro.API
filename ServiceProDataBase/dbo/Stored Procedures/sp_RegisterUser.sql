CREATE PROCEDURE sp_RegisterUser
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @PasswordHash NVARCHAR(255),
    @PhoneNumber NVARCHAR(20),
    @Role NVARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        RAISERROR('Email already exists', 16, 1)
        RETURN
    END

    INSERT INTO Users
    (Name, Email, PasswordHash, PhoneNumber, Role)
    VALUES
    (@Name, @Email, @PasswordHash, @PhoneNumber, @Role)
END
