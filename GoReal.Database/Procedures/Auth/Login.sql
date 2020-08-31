CREATE PROCEDURE [dbo].[Login]
	@Email NVARCHAR(50),
	@Password NVARCHAR(20)
AS
BEGIN
	DECLARE @State TABLE ([isActive] BIT, [isBan] BIT);
	INSERT INTO @State SELECT [isActive], [isBan] FROM [User]  WHERE [Email] = @Email AND [Password] = HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @Password + dbo.GetPostSalt())

	IF EXISTS (SELECT [isActive] FROM @State WHERE [isActive] = 0)
		RAISERROR(N'User is inactive',16,5);
	IF EXISTS (SELECT [isBan] FROM @State WHERE [isBan] = 1)
		RAISERROR(N'User is Ban',16,4);  

	SELECT [UserId], [GoTag], [LastName] , [FirstName], [Email], [Password], [isActive], [isBan], dbo.GetUserRole(@Email) AS [Role] FROM [User] WHERE [Email] = @Email AND
		[Password] = HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @Password + dbo.GetPostSalt()) AND [isActive] = 1 AND [isBan] = 0
END