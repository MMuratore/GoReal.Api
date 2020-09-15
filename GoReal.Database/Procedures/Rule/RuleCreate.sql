CREATE PROCEDURE [dbo].[RuleCreate]
	@RuleName NVARCHAR(120),
	@Overwrite BIT,
	@Suicide BIT,
	@Ko BIT
AS
BEGIN
	INSERT INTO [Rule] VALUES (@RuleName, @Overwrite, @Suicide, @Ko)
END