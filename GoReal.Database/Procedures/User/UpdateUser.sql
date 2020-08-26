CREATE PROCEDURE [dbo].[UpdateUser]
	@UserId INT,
	@GoTag NVARCHAR(120), 
	@LastName NVARCHAR(120), 
    @FirstName NVARCHAR(120), 
    @Email NVARCHAR(320), 
	@Password NVARCHAR(20)
AS
BEGIN
	UPDATE [ActiveUser] SET 
		[GoTag] = @GoTag, 
		[LastName] = @LastName,  
		[FirstName] = @FirstName, 
		[Email] = @Email, 
		[Password] = HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @Password + dbo.GetPostSalt())
	WHERE [UserId] = @UserId
END