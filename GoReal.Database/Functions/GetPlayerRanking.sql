CREATE FUNCTION [dbo].[GetPlayerRanking]( 
	@UserId INT)
RETURNS INT
AS
BEGIN
	DECLARE @Ranking TABLE ([BlackRank] INT, [WhiteRank] INT, [Result] NVARCHAR(10), [BlackId] INT, [WhiteId] INT);
	DECLARE @ActualRanking INT = 0;

	IF EXISTS (SELECT [BlackRank], [WhiteRank], [Result], [BlackPlayerId], [WhitePlayerId] FROM [Game] WHERE [Date] IN (SELECT max([Date]) FROM [Game] WHERE [Result] is not null AND ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId)))
	BEGIN
		INSERT INTO @Ranking SELECT [BlackRank], [WhiteRank], [Result], [BlackPlayerId], [WhitePlayerId] FROM [Game] WHERE [Date] IN (SELECT max([Date]) FROM [Game] WHERE [Result] is not null AND ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId))
		IF(CHARINDEX('B',(SELECT [Result] FROM @Ranking)) > 0)
		BEGIN
			IF((SELECT [BlackId] FROM @Ranking) = @UserId)
				SET @ActualRanking = (SELECT [BlackRank] FROM @Ranking) + ABS((SELECT [BlackRank] FROM @Ranking) - (SELECT [WhiteRank] FROM @Ranking))/2
			ELSE
				SET @ActualRanking = (SELECT [WhiteRank] FROM @Ranking) - ABS((SELECT [BlackRank] FROM @Ranking) - (SELECT [WhiteRank] FROM @Ranking))/2
		END
		ELSE IF(CHARINDEX('W',(SELECT [Result] FROM @Ranking)) > 0)
		BEGIN
			IF((SELECT [WhiteId] FROM @Ranking) = @UserId)
				SET @ActualRanking = (SELECT [WhiteRank] FROM @Ranking) + ABS((SELECT [BlackRank] FROM @Ranking) - (SELECT [WhiteRank] FROM @Ranking))/2
			ELSE
				SET @ActualRanking = (SELECT [BlackRank] FROM @Ranking) - ABS((SELECT [BlackRank] FROM @Ranking) - (SELECT [WhiteRank] FROM @Ranking))/2
		END
	END

	RETURN @ActualRanking
END