﻿CREATE TRIGGER [dbo].[DeleteActiveUser]
ON [User]
INSTEAD OF DELETE
AS
BEGIN
    UPDATE [User] SET [isActive] = 0 WHERE [UserId] IN (SELECT [UserId] FROM DELETED);
END