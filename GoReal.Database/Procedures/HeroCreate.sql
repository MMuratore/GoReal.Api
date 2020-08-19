CREATE PROCEDURE [dbo].[HeroCreate]
	@Name NVARCHAR(50),
	@Strength TINYINT,
	@Stamina TINYINT,
	@Intelligence TINYINT,
	@Charisma TINYINT
AS
BEGIN
	INSERT INTO [Hero] OUTPUT INSERTED.Id VALUES (@Name, @Strength, @Stamina, @Intelligence, @Charisma )
END