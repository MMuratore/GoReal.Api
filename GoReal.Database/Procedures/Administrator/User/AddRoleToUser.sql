CREATE PROCEDURE [dbo].[AddRoleToUser]
	@GoTag NVARCHAR(120),
	@RoleName NVARCHAR(120)
AS
BEGIN
	INSERT INTO [UserRole] ([UserId], [RoleId]) 
		VALUES ((SELECT [UserId] FROM [ActiveUser] WHERE [GoTag] = @GoTag), 
				(SELECT [RoleId] FROM [Role] WHERE [RoleName] = @RoleName))
END