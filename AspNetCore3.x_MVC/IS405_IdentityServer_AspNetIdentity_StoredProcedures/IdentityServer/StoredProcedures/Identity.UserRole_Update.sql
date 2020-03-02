USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserRole_Update]
GO

CREATE PROCEDURE [Identity].[UserRole_Update]
(
    @Id BIGINT,
    @Name NVARCHAR(256),
    @NormalizedName NVARCHAR(256),
    @CurrencyStamp NVARCHAR(MAX)
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    UPDATE
        [Identity].[UserRoles]
    SET
        [Name] = @Name,
        [NormalizedName] = @NormalizedName,
        [ConcurrencyStamp] = @CurrencyStamp,
        [ModifiedDatetime] = GETUTCDATE()
    WHERE
        [Id] = @Id

END
GO
