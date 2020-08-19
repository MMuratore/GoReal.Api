CREATE PROCEDURE [dbo].[HeroUpdate]
	@Id INT,
	@Name NVARCHAR(50),
	@Strength TINYINT,
	@Stamina TINYINT,
	@Intelligence TINYINT,
	@Charisma TINYINT
AS
BEGIN
	UPDATE [Hero] SET Name=@Name, Strength=@Strength, Stamina=@Stamina, Intelligence=@Intelligence, Charisma=@Charisma WHERE Id=@Id
END