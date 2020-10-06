CREATE PROCEDURE [dbo].[StatisticsGet]
	@UserId INT
AS
BEGIN
	SELECT dbo.[GetGameNumber](@UserId) AS GameNumber, dbo.[GetVictoryRatio](@UserId) AS VictoryRatio, dbo.[GetPlayTime](@UserId) AS PlayTime
END