CREATE PROCEDURE [dbo].[GameCreate]
	@Date DATETIME2,
	@Size INT,
	@Komi INT,
	@Handicap INT,
	@TimeControlId INT,
	@RuleId INT,
	@BlackPlayerId INT,
	@WhitePlayerId INT
AS
BEGIN
	INSERT INTO [Game] ([Date], [BlackRank], [WhiteRank], [Size], [Komi], [Handicap], [TimeControlId], [RuleId], [BlackPlayerId], [WhitePlayerId])
		VALUES (@Date, dbo.GetPlayerRanking(@BlackPlayerId),dbo.GetPlayerRanking(@WhitePlayerId), @Size, @Komi, @Handicap, @TimeControlId, @RuleId, @BlackPlayerId, @WhitePlayerId)
END