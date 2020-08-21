CREATE TABLE [dbo].[Role]
(
	[RoleId] INT NOT NULL IDENTITY, 
    [RoleName] NVARCHAR(120) NOT NULL, 
    CONSTRAINT [PK_Role] PRIMARY KEY ([RoleId]),
    CONSTRAINT [UK_Role_RoleName] UNIQUE ([RoleName])
)
