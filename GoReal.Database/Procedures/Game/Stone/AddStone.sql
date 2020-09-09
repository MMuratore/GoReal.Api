CREATE PROCEDURE [dbo].[AddStone]
	@GameId INT,
	@Row INT,
	@Column INT,
	@Color BIT
AS
BEGIN
	INSERT INTO [Stone] VALUES (@Row, @Column, @Color, @GameId)
END
