CREATE PROCEDURE [dbo].[GetGameNumber]
	@UserId INT
AS
BEGIN
	SELECT COUNT(GameId) FROM [Game] WHERE ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId) AND [WhitePlayerId] != [BlackPlayerId]
END