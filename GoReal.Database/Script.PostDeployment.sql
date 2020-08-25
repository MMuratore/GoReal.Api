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
EXECUTE [dbo].[CreateRole]  @RoleName = 'SuperAdministrator';
EXECUTE [dbo].[CreateRole]  @RoleName = 'Administrator';
EXECUTE [dbo].[CreateRole]  @RoleName = 'Moderator';
EXECUTE [dbo].[CreateRole]  @RoleName = 'Viewer';
EXECUTE [dbo].[CreateRole]  @RoleName = 'Player';

EXECUTE [dbo].[Register]  @GoTag = 'MuRakSss', @LastName = 'Muratore', @FirstName = 'Matthieu', 
    @Email = 'matthieu.muratore@gmail.com' , @Password = 'Test1234+';

EXECUTE [dbo].[AddRoleToUser]  @GoTag = 'MuRakSss', @RoleName = 'SuperAdministrator';

