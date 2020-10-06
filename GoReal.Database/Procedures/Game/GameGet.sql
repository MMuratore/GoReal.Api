CREATE PROCEDURE [dbo].[GameGet]
	@GameId INT
AS
BEGIN
	SELECT * FROM [Game] WHERE [GameId] = @GameId
END