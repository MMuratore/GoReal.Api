CREATE TABLE [dbo].[Rule]
(
	[RuleId] INT NOT NULL IDENTITY, 
    [RuleName] NVARCHAR(120) NOT NULL, 
    [Overwrite] BIT NOT NULL, 
    [Suicide] BIT NOT NULL, 
    [Ko] BIT NOT NULL, 
    CONSTRAINT [PK_Rule] PRIMARY KEY ([RuleId]),
    CONSTRAINT [UK_Rule_RuleName] UNIQUE ([RuleName])
)
