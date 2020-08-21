CREATE VIEW [dbo].[ActiveUser]
	AS SELECT [UserId], [GoTag], [LastName], [FirstName], [Email], [Password] FROM [User] WHERE [isActive] = 1
