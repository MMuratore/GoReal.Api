CREATE TRIGGER [dbo].[DeleteActiveUserRole]
ON [UserRole]
INSTEAD OF DELETE
AS
BEGIN
    DECLARE @Id INT;
    SET @Id = (SELECT [UserId] FROM DELETED);
    UPDATE [UserRole] SET [isActive] = 0 WHERE [UserId] = @Id;
END
