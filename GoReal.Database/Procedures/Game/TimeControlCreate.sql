CREATE PROCEDURE [dbo].[TimeControlCreate]
	@Speed NVARCHAR(50),
	@OverTime NVARCHAR(50),
	@TimeLimit INT = NULL,
	@TimePerPeriod INT = NULL,
	@Period INT = NULL,
	@InitalTime INT = NULL
AS
BEGIN
	INSERT INTO [TimeControl] VALUES (@Speed, @OverTime, @TimeLimit, @TimePerPeriod, @Period, @InitalTime)
END