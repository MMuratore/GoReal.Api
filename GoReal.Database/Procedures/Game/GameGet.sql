CREATE PROCEDURE [dbo].[GameGet]
	@GameId INT
AS
BEGIN
	SELECT [GameId], [Date], [BlackRank], [WhiteRank], [Result], [Size], [Komi], [Handicap], [BlackCapture], [WhiteCapture], [KoInfo], [TimeControlId], [RuleId], [BlackPlayerId], [WhitePlayerId] FROM [Game] WHERE [GameId] = @GameId
END