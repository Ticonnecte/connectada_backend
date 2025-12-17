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


ALTER TABLE PREFEITURA ADD S3_REGION VARCHAR(25)
GO

UPDATE PREFEITURA
SET S3_REGION = 's3.us-east-1'
GO

ALTER TABLE PREFEITURA ADD S3_ACCESS_KEY_ID VARCHAR(50), S3_ACCESS_KEY_SECRET VARCHAR(50);
