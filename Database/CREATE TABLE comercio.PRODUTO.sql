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


CREATE TABLE comercio.PRODUTO(
    ID VARCHAR(128) NOT NULL PRIMARY KEY CLUSTERED,
    COMERCIO_ID VARCHAR(128) NOT NULL REFERENCES comercio.COMERCIO(ID),
    NOME VARCHAR(50) NOT NULL,
    DESCRICAO VARCHAR(400),
    IMG_URL VARCHAR(255),
    VALOR DECIMAL(10, 2),
    TENANT_KEY INT
)
GO
