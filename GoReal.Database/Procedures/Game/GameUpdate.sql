CREATE PROCEDURE [dbo].[GameUpdate]
	@GameId INT,
	@BlackCapture INT, 
	@WhiteCapture INT, 
    @KoInfo NVARCHAR(30)
AS
BEGIN
	UPDATE [Game] SET 
		[BlackCapture] = @BlackCapture,  
		[WhiteCapture] = @WhiteCapture, 
		[KoInfo] = @KoInfo
	WHERE [GameId] = @GameId
END