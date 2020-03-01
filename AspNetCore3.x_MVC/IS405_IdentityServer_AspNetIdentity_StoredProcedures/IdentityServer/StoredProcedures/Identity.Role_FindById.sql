USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[Role_FindById]
GO

CREATE PROCEDURE [Identity].[Role_FindById]
(
    @RoleId INT
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
        [RoleId] = @RoleId

END
GO
