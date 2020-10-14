CREATE PROCEDURE [dbo].[RuleCreate]
	@RuleName NVARCHAR(120),
	@Overwrite BIT,
	@Suicide BIT,
	@Ko BIT
AS
BEGIN
	INSERT INTO [Rule] VALUES (@RuleName, @Overwrite, @Suicide, @Ko)
	SELECT * FROM [Rule] WHERE [RuleId] = CAST(SCOPE_IDENTITY() AS INT)
END