USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserRole_Create]
GO

CREATE PROCEDURE [Identity].[UserRole_Create]
(
    @Name NVARCHAR(256),
    @NormalizedName NVARCHAR(256),
    @ConcurrencyStamp NVARCHAR(256)
)
AS
BEGIN

    INSERT INTO [Identity].[UserRoles]
    (
        [Name],
        [NormalizedName],
        [ConcurrencyStamp],
        [CreatedDatetime],
        [ModifiedDateTime]
    )
    VALUES
    (
        @Name,
        @NormalizedName,
        @ConcurrencyStamp,
        GETUTCDATE(),
        GETUTCDATE()
    )

    SELECT SCOPE_IDENTITY()

END
GO
