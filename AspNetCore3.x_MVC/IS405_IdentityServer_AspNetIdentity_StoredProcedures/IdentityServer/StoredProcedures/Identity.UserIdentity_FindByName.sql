USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserIdentity_FindByName]
GO

CREATE PROCEDURE [Identity].[UserIdentity_FindByName]
(
    @NormalizedUserName VARCHAR(256)
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
        [NormalizedUserName] = @NormalizedUserName

END
GO
