USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[Role_FindByName]
GO

CREATE PROCEDURE [Identity].[Role_FindByName]
(
    @RoleName NVARCHAR(256)
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT
        [RoleId],
        [Name],
        [NormalizedName],
        [ConcurrencyStamp],
        [RoleName]
    FROM
        [Identity].[Roles]
    WHERE
        [NormalizedName] = @RoleName

END
GO
