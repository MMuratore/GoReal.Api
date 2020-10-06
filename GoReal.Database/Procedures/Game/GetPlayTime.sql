CREATE PROCEDURE [dbo].[GetPlayTime]
	@UserId INT
AS
BEGIN
	SELECT [StartDate], [EndDate] FROM [Game] WHERE ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId) AND [EndDate] IS NOT NULL
END