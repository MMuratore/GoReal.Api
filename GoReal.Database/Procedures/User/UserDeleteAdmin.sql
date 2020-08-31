CREATE PROCEDURE [dbo].[UserDeleteAdmin]
	@UserId INT
AS
BEGIN
	DELETE FROM [UserRole] WHERE [UserId] = @UserId;
	DELETE FROM [User] WHERE [UserId] = @UserId;
END