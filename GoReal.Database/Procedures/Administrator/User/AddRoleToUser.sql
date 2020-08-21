CREATE PROCEDURE [dbo].[AddRoleToUser]
	@UserId INT,
	@RoleName NVARCHAR(120)
AS
BEGIN
	INSERT INTO [UserRole] ([UserId], [RoleId]) VALUES (@UserId, (SELECT [RoleId] FROM [Role] WHERE [RoleName] = @RoleName))
END