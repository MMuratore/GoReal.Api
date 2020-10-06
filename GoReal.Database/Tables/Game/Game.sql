CREATE TABLE [dbo].[Game]
(
	[GameId] INT NOT NULL IDENTITY, 
    [StartDate] DATETIME2(0) NOT NULL, 
    [EndDate] DATETIME2(0) NULL, 
    [BlackRank] INT NOT NULL, 
    [WhiteRank] INT NOT NULL, 
    [Result] NVARCHAR(10) NULL, 
    [Size] INT NOT NULL, 
    [Komi] INT NOT NULL, 
    [Handicap] INT NOT NULL, 
    [BlackCapture] INT NOT NULL DEFAULT 0,
    [WhiteCapture] INT NOT NULL DEFAULT 0,
    [BlackState] BIT NULL DEFAULT 1,
    [WhiteState] BIT NULL DEFAULT 0,
    [KoInfo] NVARCHAR(30) NULL DEFAULT '-1,-1,null' ,
    [TimeControlId] INT NOT NULL, 
    [RuleId] INT NOT NULL, 
    [BlackPlayerId] INT NOT NULL, 
    [WhitePlayerId] INT NOT NULL, 
    CONSTRAINT [PK_Game] PRIMARY KEY ([GameId]),
    CONSTRAINT [FK_Game_TimeControl] FOREIGN KEY ([TimeControlId]) REFERENCES [TimeControl] ([TimeControlId]),
    CONSTRAINT [FK_Game_Rule] FOREIGN KEY ([RuleId]) REFERENCES [Rule] ([RuleId]),
    CONSTRAINT [FK_Game_UserBlack] FOREIGN KEY ([BlackPlayerId]) REFERENCES [User] ([UserId]),
    CONSTRAINT [FK_Game_UserWhite] FOREIGN KEY ([WhitePlayerId]) REFERENCES [User] ([UserId]),
    CONSTRAINT [CK_Game_Size] CHECK ([Size]=9 OR [Size]=13 OR [Size]=19),
    CONSTRAINT [CK_Game_EndDate] CHECK ([EndDate] IS NOT NULL OR [EndDate] >= [StartDate])
--    CONSTRAINT [CK_Game_Result] CHECK ([Result] LIKE '[BW][+]([R]|([0-9]([.][5])?))$')
)
