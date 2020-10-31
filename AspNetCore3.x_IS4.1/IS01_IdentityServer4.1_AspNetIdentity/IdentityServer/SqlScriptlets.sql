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