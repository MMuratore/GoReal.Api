CREATE PROCEDURE [dbo].[DeleteUser]
	@UserId INT
AS
BEGIN
	DELETE FROM [ActiveUser] WHERE [UserId] = @UserId;
END

