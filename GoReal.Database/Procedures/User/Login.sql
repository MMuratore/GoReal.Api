CREATE PROCEDURE [dbo].[Login]
	@Email NVARCHAR(50),
	@Password NVARCHAR(20)
AS
BEGIN
	SELECT * FROM [User] WHERE [Email] = @Email AND
		[Password] = HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @Password + dbo.GetPostSalt())
END