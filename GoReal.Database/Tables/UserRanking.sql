CREATE TABLE [dbo].[UserRanking]
(
	[UserRankingId] INT NOT NULL IDENTITY, 
    [Ranking] INT NOT NULL, 
    [ValidFrom] DATETIME2 NOT NULL, 
    [ValidTo] DATETIME2 NULL,
    [UserId] INT NOT NULL, 
    CONSTRAINT [PK_UserRanking] PRIMARY KEY ([UserRankingId]),
    CONSTRAINT [FK_UserRanking_User] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId])
)
