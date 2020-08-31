CREATE PROCEDURE [dbo].[UserActivate]
	@UserId INT
AS
BEGIN
    UPDATE [User] SET [isActive] = 1 WHERE [UserId] = @UserId;
END
