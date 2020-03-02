USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserRole_FindById]
GO

CREATE PROCEDURE [Identity].[UserRole_FindById]
(
    @Id BIGINT
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
        [Id] = @Id

END
GO
