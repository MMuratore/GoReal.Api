CREATE TABLE [dbo].[User]
(
	[UserId] INT NOT NULL IDENTITY, 
    [GoTag] NVARCHAR(120) NOT NULL,
    [LastName] NVARCHAR(320) NULL, 
    [FirstName] NVARCHAR(320) NULL, 
    [Email] NVARCHAR(320) NOT NULL, 
    [Password] BINARY(64) NOT NULL, 
    [isAdmin] BIT NOT NULL DEFAULT 0, 
    [isActive] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_User] PRIMARY KEY ([UserId]),
    CONSTRAINT [UC_User_Email] UNIQUE ([Email])
)
