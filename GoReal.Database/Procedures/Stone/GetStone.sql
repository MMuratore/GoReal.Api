CREATE PROCEDURE [dbo].[GetStone]
	@GameId INT
AS
BEGIN
	SELECT [Row], [Column], [Color] FROM [Stone] WHERE [GameId] = @GameId
END
