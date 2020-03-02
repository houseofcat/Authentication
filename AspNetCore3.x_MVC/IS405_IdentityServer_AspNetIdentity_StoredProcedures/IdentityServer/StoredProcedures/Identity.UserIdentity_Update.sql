USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserIdentity_Update]
GO

CREATE PROCEDURE [Identity].[UserIdentity_Update]
(
    @UserId BIGINT,
    @UserName NVARCHAR(256),
    @NormalizedUserName NVARCHAR(256),
    @Email NVARCHAR(256),
    @NormalizedEmail NVARCHAR(256),
    @EmailConfirmed BIT,
    @PasswordHash NVARCHAR(MAX),
    @SecurityStamp NVARCHAR(MAX),
    @ConcurrencyStamp NVARCHAR(MAX),
    @PhoneNumber NVARCHAR(MAX),
    @PhoneNumberConfirmed BIT,
    @TwoFactorEnabled BIT,
    @LockoutEnd DATETIMEOFFSET(7),
    @LockoutEnabled BIT,
    @AccessFailedCount INT,
    @RegionId INT,
    @CurrentRegionId INT,
    @IsActive BIT
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    UPDATE
        [Identity].[UserIdentities]
    SET
        [UserName] = @UserName,
        [NormalizedUserName] = @NormalizedUserName,
        [Email] = @Email,
        [NormalizedEmail] = @NormalizedEmail,
        [EmailConfirmed] = @EmailConfirmed,
        [PasswordHash] = @PasswordHash,
        [SecurityStamp] = @SecurityStamp,
        [ConcurrencyStamp] = @ConcurrencyStamp,
        [PhoneNumber] = @PhoneNumber,
        [PhoneNumberConfirmed] = @PhoneNumberConfirmed,
        [TwoFactorEnabled] = @TwoFactorEnabled,
        [LockoutEnd] = @LockoutEnd,
        [LockoutEnabled] = @LockoutEnabled,
        [AccessFailedCount] = @AccessFailedCount,
        [RegionId] = @RegionId,
        [CurrentRegionId] = @CurrentRegionId,
        [ModifiedDatetime] = GETUTCDATE(),
        [IsActive] = @IsActive
    WHERE
        [UserId] = @UserId

END
GO
