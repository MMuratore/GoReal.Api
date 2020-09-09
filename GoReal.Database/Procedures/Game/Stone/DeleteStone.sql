CREATE PROCEDURE [dbo].[DeleteStone]
	@GameId INT,
	@Row INT,
	@Column INT
AS
BEGIN
	DELETE FROM [Stone] WHERE [Row] = @Row AND 
		[Column] = @Column AND [GameId] = @GameId
END
