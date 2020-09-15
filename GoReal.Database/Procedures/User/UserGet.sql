CREATE PROCEDURE [dbo].[UserGet]
	@UserId INT
AS
BEGIN
	SELECT [UserId], [GoTag], [LastName] , [FirstName], [Email], [Password], [isActive], [isBan], dbo.GetUserRole(UserId) AS [Role] FROM [User] WHERE [UserId] = @UserId
END