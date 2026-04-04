CREATE PROCEDURE sp_LoginUser  
    @Email NVARCHAR(150)  
AS  
BEGIN  
    SELECT   
        Id,  
        Name,  
        Email,  
        PasswordHash,  
        Role,
        PhoneNumber
    FROM Users  
    WHERE Email = @Email  
END  