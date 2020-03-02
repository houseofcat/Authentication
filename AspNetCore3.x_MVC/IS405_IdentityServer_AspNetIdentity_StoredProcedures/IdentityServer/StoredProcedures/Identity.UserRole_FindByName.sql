USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserRole_FindByName]
GO

CREATE PROCEDURE [Identity].[UserRole_FindByName]
(
    @NormalizedName NVARCHAR(256)
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT
        [Id],
        [Name],
        [NormalizedName],
        [ConcurrencyStamp],
        [CreatedDatetime],
        [ModifiedDatetime]
    FROM
        [Identity].[UserRoles]
    WHERE
        [NormalizedName] = @NormalizedName

END
GO
