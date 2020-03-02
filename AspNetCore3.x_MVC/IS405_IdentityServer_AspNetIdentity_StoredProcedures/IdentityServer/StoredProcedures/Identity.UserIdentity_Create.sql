USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserIdentity_Create]
GO

CREATE PROCEDURE [Identity].[UserIdentity_Create]
(
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

    INSERT INTO [Identity].[UserIdentities]
    (
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
    )
    VALUES
    (
        @UserName,
        @NormalizedUserName,
        @Email,
        @NormalizedEmail,
        @EmailConfirmed,
        @PasswordHash,
        @SecurityStamp,
        @ConcurrencyStamp,
        @PhoneNumber,
        @PhoneNumberConfirmed,
        @TwoFactorEnabled,
        @LockoutEnd,
        @LockoutEnabled,
        @AccessFailedCount,
        @RegionId,
        @CurrentRegionId,
        GETUTCDATE(),
        GETUTCDATE(),
        @IsActive
    )

    SELECT SCOPE_IDENTITY()
END
GO
