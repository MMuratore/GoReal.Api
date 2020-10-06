/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
EXECUTE [dbo].[RoleCreate]  @RoleName = 'SuperAdministrator';
EXECUTE [dbo].[RoleCreate]  @RoleName = 'Administrator';
EXECUTE [dbo].[RoleCreate]  @RoleName = 'Moderator';
EXECUTE [dbo].[RoleCreate]  @RoleName = 'Viewer';
EXECUTE [dbo].[RoleCreate]  @RoleName = 'Player';

EXECUTE [dbo].[Register]  @GoTag = 'MuRakSss', @LastName = 'Muratore', @FirstName = 'Matthieu', 
    @Email = 'matthieu.muratore@gmail.com' , @Password = 'Test1234+';
EXECUTE [dbo].[Register]  @GoTag = 'CasTou', @LastName = 'Hanuise', @FirstName = 'Manon', 
    @Email = 'manonhanuise@gmail.com' , @Password = 'Test1234+';

EXECUTE [dbo].[AddRoleToUser]  @GoTag = 'MuRakSss', @RoleName = 'SuperAdministrator';

EXECUTE [dbo].[RuleCreate]  @RuleName = 'Japanese', @Overwrite = 1, @Suicide = 1, @Ko = 1;

EXECUTE [dbo].[TimeControlCreate] @Speed = 'Live', @OverTime = 'Byo-Yomi', @TimeLimit = 600,
    @TimePerPeriod = 30, @Period = 5;
EXECUTE [dbo].[TimeControlCreate] @Speed = 'Blitz', @OverTime = 'Byo-Yomi', @TimeLimit = 30,
    @TimePerPeriod = 5, @Period = 5;
EXECUTE [dbo].[TimeControlCreate] @Speed = 'Live', @OverTime = 'Simple', @TimePerPeriod = 30;
EXECUTE [dbo].[TimeControlCreate] @Speed = 'Blitz', @OverTime = 'Simple', @TimePerPeriod = 5; 
EXECUTE [dbo].[TimeControlCreate] @Speed = 'Live', @OverTime = 'Absolute', @TimeLimit = 1800; 
EXECUTE [dbo].[TimeControlCreate] @Speed = 'Blitz', @OverTime = 'Absolute',  @TimeLimit = 600;

INSERT INTO [Game]
		VALUES (GETDATE(), '2020-09-03', dbo.GetPlayerRanking(1), dbo.GetPlayerRanking(1), null, 9, 0, 0, 0, 0, null, null, '-1,-1,null', 1, 1, 1, 1);
INSERT INTO [Game]
		VALUES ('2020-09-03', '2020-09-04', 100, 100, 'W+R', 9, 0, 0, 0, 0, null, null,'-1,-1,null', 1, 1, 1, 2);
INSERT INTO [Game]
		VALUES ('2020-09-04', '2020-09-05', 50, 150, 'W+12', 9, 0, 0, 5, 8, null, null, '-1,-1,null', 1, 1, 1, 2);
INSERT INTO [Game]
		VALUES ('2020-09-04', '2020-09-05', 0, 200, 'B+5', 9, 0, 0, 5, 12, null, null, '-1,-1,null', 1, 1, 1, 2);

EXECUTE[dbo].[GameCreate] @Size = 9, @Komi = 0, @Handicap = 0, @TimeControlId = 2,
		@RuleId = 1, @BlackPlayerId = 2, @WhitePlayerId = 1;

EXECUTE [dbo].[AddStone] @GameId = 3, @Row = 1, @Column = 0, @Color = 0;
EXECUTE [dbo].[AddStone] @GameId = 3, @Row = 0, @Column = 1, @Color = 0;
EXECUTE [dbo].[AddStone] @GameId = 3, @Row = 1, @Column = 2, @Color = 0;

EXECUTE [dbo].[AddStone] @GameId = 3, @Row = 2, @Column = 0, @Color = 1;
EXECUTE [dbo].[AddStone] @GameId = 3, @Row = 1, @Column = 1, @Color = 1;
EXECUTE [dbo].[AddStone] @GameId = 3, @Row = 3, @Column = 1, @Color = 1;
EXECUTE [dbo].[AddStone] @GameId = 3, @Row = 2, @Column = 2, @Color = 1;
