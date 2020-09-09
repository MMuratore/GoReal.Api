CREATE TABLE [dbo].[TimeControl]
(
	[TimeControlId] INT NOT NULL IDENTITY, 
    [Speed] NVARCHAR(50) NOT NULL, 
    [OverTime] NVARCHAR(50) NOT NULL, 
    [TimeLimit] INT NULL, 
    [TimePerPeriod] INT NULL, 
    [Period] INT NULL, 
    [InitialTime] INT NULL, 
    CONSTRAINT [PK_TimeControl] PRIMARY KEY ([TimeControlId])
)
