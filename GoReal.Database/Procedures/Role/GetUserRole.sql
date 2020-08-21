CREATE PROCEDURE [dbo].[GetUserRole]
	@UserId INT
AS
BEGIN
	SELECT R.[RoleName] FROM [Role] AS R JOIN [ActiveUserRole] AS U 
		ON R.[RoleId] = U.[RoleId]
			WHERE U.[UserId] = @UserId
END
