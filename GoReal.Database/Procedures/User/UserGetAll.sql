CREATE PROCEDURE [dbo].[UserGetAll]
AS
BEGIN
	SELECT [UserId], [GoTag], [LastName] , [FirstName], [Email], [Password], [isActive], [isBan], dbo.GetUserRole(UserId) AS [Role] FROM [User]
END