CREATE PROCEDURE [dbo].[UserDelete]
	@UserId INT
AS
BEGIN
	UPDATE [User] SET [isActive] = 0 WHERE [UserId] = @UserId;
END