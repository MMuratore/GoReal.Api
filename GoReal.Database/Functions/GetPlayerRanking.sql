CREATE FUNCTION [dbo].[GetPlayerRanking]( 
	@UserId INT)
RETURNS INT
AS
BEGIN
	DECLARE @Ranking TABLE ([BlackRank] INT, [WhiteRank] INT, [Result] NVARCHAR(10), [BlackId] INT, [WhiteId] INT);
	DECLARE @ActualRanking INT = 0;
	DECLARE @Gain INT = 0;

	IF EXISTS (SELECT [BlackRank], [WhiteRank], [Result], [BlackPlayerId], [WhitePlayerId] FROM [Game] WHERE [StartDate] IN (SELECT max([StartDate]) FROM [Game] WHERE [Result] is not null AND ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId)))
	BEGIN
		INSERT INTO @Ranking SELECT TOP 1 [BlackRank], [WhiteRank], [Result], [BlackPlayerId], [WhitePlayerId] FROM [Game] WHERE [StartDate] IN (SELECT max([StartDate]) FROM [Game] WHERE [Result] is not null AND ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId)) ORDER BY [GameId] DESC
		SET @Gain = ABS((SELECT [BlackRank] FROM @Ranking) - (SELECT [WhiteRank] FROM @Ranking))/2;
		IF(@Gain = 0)
		BEGIN
			SET @Gain = 50;
		END
		IF(CHARINDEX('B',(SELECT [Result] FROM @Ranking)) > 0)
		BEGIN
			IF((SELECT [BlackId] FROM @Ranking) = @UserId)
				SET @ActualRanking = (SELECT [BlackRank] FROM @Ranking) + @Gain
			ELSE
				SET @ActualRanking = (SELECT [WhiteRank] FROM @Ranking) - @Gain
		END
		ELSE IF(CHARINDEX('W',(SELECT [Result] FROM @Ranking)) > 0)
		BEGIN
			IF((SELECT [WhiteId] FROM @Ranking) = @UserId)
				SET @ActualRanking = (SELECT [WhiteRank] FROM @Ranking) + @Gain
			ELSE
				SET @ActualRanking = (SELECT [BlackRank] FROM @Ranking) - @Gain
		END
		IF(@ActualRanking < 0)
		BEGIN
			SET @ActualRanking = 0;
		END
	END

	RETURN @ActualRanking
END