CREATE TABLE [dbo].[UserRole]
(
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL, 
    [isActive] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_UserRole] PRIMARY KEY ([UserId],[RoleId]),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [User] ([UserId]),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId])
)
