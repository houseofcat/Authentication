------------------------------------
------------------------------------

--IdentityServer's Logs
SELECT TOP (1000) [Id]
      ,[Message]
      ,[MessageTemplate]
      ,[Level]
      ,[TimeStamp]
      ,[Exception]
      ,[Properties]
FROM [Identity].[Log].[IdentityServer]
--WHERE [Message] LIKE '%test%'
  ORDER BY Id DESC

------------------------------------
------------------------------------

-- Creating a bunch of DROP table scripts as the output
-- Based on the Schema 'Identity' stored in @Schema
USE [Identity]

DECLARE @SqlStatement NVARCHAR(MAX)
DECLARE @Schema VARCHAR(32) = 'Identity'

SELECT @SqlStatement = 
    COALESCE(@SqlStatement, N'') + N'DROP TABLE IF EXISTS [' + @Schema +'].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @Schema and TABLE_TYPE = 'BASE TABLE'

PRINT @SqlStatement

------------------------------------
------------------------------------

-- Remove EF Migrations for replay on next startup.

TRUNCATE TABLE [dbo].[__EFMigrationsHistory]
GO -- No Id column