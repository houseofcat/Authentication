USE IdentityLogging

--IdentityServer's Logs
SELECT TOP (1000) [Id]
      ,[Message]
      ,[MessageTemplate]
      ,[Level]
      ,[TimeStamp]
      ,[Exception]
      ,[Properties]
FROM [IdentityLogging].[dbo].[IdentityServer]
  ORDER BY Id DESC