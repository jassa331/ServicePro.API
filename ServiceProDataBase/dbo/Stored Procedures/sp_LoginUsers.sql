CREATE PROCEDURE sp_LoginUsers
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
