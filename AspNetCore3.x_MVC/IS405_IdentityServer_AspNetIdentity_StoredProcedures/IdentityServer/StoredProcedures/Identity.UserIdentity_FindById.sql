USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserIdentity_FindById]
GO

CREATE PROCEDURE [Identity].[UserIdentity_FindById]
(
    @UserId BIGINT
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT
        [UserId],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumber],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnd],
        [LockoutEnabled],
        [AccessFailedCount],
        [RegionId],
        [CurrentRegionId],
        [CreatedDatetime],
        [ModifiedDatetime],
        [IsActive]
    FROM
        [Identity].[UserIdentities]
    WHERE
        [UserId] = @UserId

END
GO
