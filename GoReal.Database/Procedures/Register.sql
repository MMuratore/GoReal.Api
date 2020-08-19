CREATE PROCEDURE [dbo].[Register]
	@LastName NVARCHAR(50), 
    @FirstName NVARCHAR(50), 
    @Email NVARCHAR(320), 
	@password NVARCHAR(20)
AS
BEGIN
	INSERT INTO [User] VALUES (@LastName, @FirstName, @Email, HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @password + dbo.GetPostSalt())  )
END