CREATE FUNCTION [dbo].[GetPlayTime]( 
	@UserId INT)
RETURNS INT
AS
BEGIN
	DECLARE @TimeTable TABLE ([Id] [INT] IDENTITY(1,1) NOT NULL, [DateDiff] INT);
	DECLARE @PlayTime INT = 0;
	DECLARE @Counter INT 

	INSERT INTO @TimeTable SELECT DATEDIFF(MINUTE, [StartDate], [EndDate]) FROM [Game] WHERE ([BlackPlayerId] = 1 OR [WhitePlayerId] = 1) AND [EndDate] IS NOT NULL;
	SET @Counter = (SELECT COUNT([Id]) FROM @TimeTable);
	WHILE ( @Counter > 0)
	BEGIN
		SET @PlayTime = @PlayTime + (SELECT [DateDiff] FROM @TimeTable WHERE [Id] = @Counter)
		SET @Counter  = @Counter  - 1;
	END
	RETURN @PlayTime;
END