CREATE PROCEDURE [dbo].[Register]
	@GoTag NVARCHAR(120), 
	@LastName NVARCHAR(320), 
    @FirstName NVARCHAR(320), 
    @Email NVARCHAR(320), 
	@Password NVARCHAR(20)
AS
BEGIN
	DECLARE @Id TABLE (Id INT, Ranking INT)
	BEGIN TRY
		BEGIN TRANSACTION
		 INSERT INTO [User] ([GoTag], [LastName], [FirstName], [Email], [Password])
			OUTPUT inserted.UserId, inserted.Ranking INTO @Id
			VALUES (@GoTag, @LastName, @FirstName, @Email, 
				HASHBYTES ( 'SHA2_512', dbo.GetPreSalt() + @Password + dbo.GetPostSalt()))
		INSERT INTO [UserRanking] ([Ranking], [ValidFrom], [UserId]) 
			VALUES ((SELECT Ranking FROM @Id), GETDATE(), (SELECT Id FROM @Id))  
		COMMIT TRANSACTION
	END TRY  
	BEGIN CATCH  
		 ROLLBACK TRANSACTION;  
	END CATCH  
END