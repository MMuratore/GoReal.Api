CREATE PROCEDURE [dbo].[Register]
	@GoTag NVARCHAR(120), 
	@LastName NVARCHAR(320), 
    @FirstName NVARCHAR(320), 
    @Email NVARCHAR(320), 
	@Password NVARCHAR(20)
AS
BEGIN
	INSERT INTO [User] ([GoTag], [LastName], [FirstName], [Email], [Password])
		OUTPUT inserted.UserId
		VALUES (@GoTag, @LastName, @FirstName, @Email, 
			HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @Password + dbo.GetPostSalt()))
END