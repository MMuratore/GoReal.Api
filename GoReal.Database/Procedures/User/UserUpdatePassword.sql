CREATE PROCEDURE [dbo].[UserUpdatePassword]
	@UserId INT,
	@Password NVARCHAR(20)
AS
BEGIN
	UPDATE [User] SET 
		[Password] = HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @Password + dbo.GetPostSalt())
	WHERE [UserId] = @UserId
END