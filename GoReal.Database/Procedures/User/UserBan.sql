CREATE PROCEDURE [dbo].[UserBan]
	@UserId INT
AS
BEGIN
    UPDATE [User] SET [isBan] = ~[isBan] WHERE [UserId] = @UserId;
END