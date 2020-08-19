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
EXECUTE [dbo].[Register]  @GoTag = 'MuRakSss', @LastName = 'Muratore', @FirstName = 'Matthieu', 
    @Email = 'matthieu.muratore@gmail.com' , @Password = 'Test1234+';
UPDATE [dbo].[User] SET [isAdmin] = 1 WHERE [GoTag] = 'MuRakSss';
