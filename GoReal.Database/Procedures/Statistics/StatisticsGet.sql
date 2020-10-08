CREATE PROCEDURE [dbo].[StatisticsGet]
	@UserId INT
AS
BEGIN
	SELECT @UserId AS UserId,
		dbo.[GetGameNumber](@UserId) AS GameNumber, 
		dbo.[GetTrainingNumber](@UserId) AS TrainingNumber, 
		dbo.[GetVictoryRatio](@UserId) AS VictoryRatio, 
		dbo.[GetPlayTime](@UserId) AS PlayTime
END