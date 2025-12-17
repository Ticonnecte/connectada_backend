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
CREATE SCHEMA info
GO

ALTER TABLE NOTICIA_CATEGORIA DROP COLUMN [IND_INFO_SAIBA_MAIS]
GO

CREATE TABLE [info].[CATEGORIA](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
	[NOME] [varchar](50) NULL,
	[COR] [int] NULL,
	[DESCRICAO] [varchar](400) NULL,
	[ICONE_NOME] [varchar](255) NULL,
	ATIVA BIT NULL,
	[TENANT_ID] [int] NULL
)
GO

CREATE TABLE [info].[INFO](
	[ID] [varchar](128) NOT NULL  PRIMARY KEY CLUSTERED,
	[LEAD] [varchar](400) NULL,
	[CONTEUDO] text NULL,
	[TENANT_ID] [int] NULL,
	--[DH_CRIACAO] [smalldatetime] NOT NULL,
	--[DH_ULTIMO_UPD] [smalldatetime] NULL,
	--[USUARIO_INS] VARCHAR(100) NOT NULL,
	--[USUARIO_UPD] VARCHAR(100) NULL,
	--[VERSION] TIMESTAMP,
	[FOTO_CAPA_URL] [varchar](max) NULL,
	ATIVAS Bit,
	[CATEGORIA_ID] INT NOT NULL REFERENCES info.CATEGORIA(ID)
)
GO




