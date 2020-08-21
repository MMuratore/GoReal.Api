CREATE VIEW [dbo].[ActiveUserRole]
	AS SELECT [UserId], [RoleId] FROM [UserRole] WHERE [isActive] = 1
