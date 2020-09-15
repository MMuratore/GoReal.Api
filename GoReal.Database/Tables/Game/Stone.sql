CREATE TABLE [dbo].[Stone]
(
    [Row] INT NOT NULL, 
    [Column] INT NOT NULL, 
    [Color] BIT NOT NULL, 
    [GameId] INT NOT NULL, 
    CONSTRAINT [PK_Stone] PRIMARY KEY ([GameId], [Row], [Column]),
    CONSTRAINT [FK_Stone_Game] FOREIGN KEY ([GameId]) REFERENCES [Game] ([GameId])
)
