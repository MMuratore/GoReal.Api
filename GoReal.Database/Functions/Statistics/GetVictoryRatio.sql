CREATE FUNCTION [dbo].[GetVictoryRatio]( 
	@UserId INT)
RETURNS DECIMAL (9,2)
AS
BEGIN
	DECLARE @Victory INT = 0;
	DECLARE @GameNumber INT = 0;

	SET @GameNumber = (SELECT COUNT(GameId) FROM [Game] WHERE ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId) AND [WhitePlayerId] != [BlackPlayerId] AND [Result] IS NOT NULL)
	SET @Victory = (SELECT COUNT(GameId) FROM [Game] WHERE (([BlackPlayerId] = @UserId AND [Result] LIKE 'B%') OR ([WhitePlayerId] = @UserId AND [Result] LIKE 'W%'))
			AND [WhitePlayerId] != [BlackPlayerId] AND [Result] IS NOT NULL)
	
	IF( @GameNumber = 0)
		RETURN 0;
	RETURN @Victory/CAST(@GameNumber AS DECIMAL (9,2));
END