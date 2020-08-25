CREATE PROCEDURE [dbo].[DeleteUser]
	@UserId INT
AS
BEGIN
	DELETE FROM [User] WHERE [UserId] = @UserId;
END

