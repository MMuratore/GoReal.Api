CREATE PROCEDURE [dbo].[GameUpdate]
	@GameId INT,
	@Result NVARCHAR(10),
	@BlackCapture INT, 
	@WhiteCapture INT,
	@BlackState BIT,
	@WhiteState BIT,
    @KoInfo NVARCHAR(30)
AS
BEGIN
	UPDATE [Game] SET  
		[Result] = @Result, 
		[BlackCapture] = @BlackCapture,  
		[WhiteCapture] = @WhiteCapture, 
		[BlackState] = @BlackState,  
		[WhiteState] = @WhiteState, 
		[KoInfo] = @KoInfo
	WHERE [GameId] = @GameId
END