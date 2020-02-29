-- Search IdentityServer's logs
USE [IdentityLogging]

--IdentityServer's Logs
SELECT TOP (1000) [Id]
      ,[Message]
      ,[MessageTemplate]
      ,[Level]
      ,[TimeStamp]
      ,[Exception]
      ,[Properties]
FROM [IdentityLogging].[dbo].[IdentityServer]
--WHERE [Message] LIKE '%test%'
  ORDER BY Id DESC

------------------------------------
------------------------------------

-- Creating a bunch of DROP table scripts as the output
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