CREATE PROCEDURE [dbo].[GetPlayTime]
	@userId INT
AS
BEGIN
	SELECT [StartDate], [EndDate] FROM [Game] WHERE ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId) AND [EndDate] IS NOT NULL
END