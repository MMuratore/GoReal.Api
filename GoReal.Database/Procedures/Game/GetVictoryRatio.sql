CREATE PROCEDURE [dbo].[GetVictoryRatio]
	@UserId INT
AS
BEGIN
	DECLARE @Victory INT = 0;
	DECLARE @GameNumber INT = 0;

	SET @GameNumber = (SELECT COUNT(GameId) FROM [Game] WHERE ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId) AND [WhitePlayerId] != [BlackPlayerId] AND [Result] IS NOT NULL)
	SET @Victory = (SELECT COUNT(GameId) FROM [Game] WHERE (([BlackPlayerId] = @UserId AND [Result] LIKE 'B%') OR ([WhitePlayerId] = @UserId AND [Result] LIKE 'W%'))
			AND [WhitePlayerId] != [BlackPlayerId] AND [Result] IS NOT NULL)

	SELECT @Victory/CAST(@GameNumber AS DECIMAL (9,2))
END