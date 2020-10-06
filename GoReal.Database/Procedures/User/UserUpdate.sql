CREATE PROCEDURE [dbo].[UserUpdate]
	@UserId INT,
	@GoTag NVARCHAR(120), 
	@LastName NVARCHAR(120), 
    @FirstName NVARCHAR(120), 
    @Email NVARCHAR(320)
AS
BEGIN
	UPDATE [User] SET 
		[GoTag] = @GoTag, 
		[LastName] = @LastName,  
		[FirstName] = @FirstName, 
		[Email] = @Email
	WHERE [UserId] = @UserId
END