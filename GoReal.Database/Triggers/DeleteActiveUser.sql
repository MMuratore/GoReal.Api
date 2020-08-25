CREATE TRIGGER [dbo].[DeleteActiveUser]
ON [User]
INSTEAD OF DELETE
AS
BEGIN
    DECLARE @Id INT;
    SET @Id = (SELECT [UserId] FROM DELETED);
    UPDATE [User] SET [isActive] = 0 WHERE [UserId] = @Id;
END