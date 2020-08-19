CREATE PROCEDURE [dbo].[Login]
	@email NVARCHAR(50),
	@password NVARCHAR(20)
AS
BEGIN
	SELECT * FROM [User] WHERE [Email] = @email AND [Password] = HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @password + dbo.GetPostSalt())
END