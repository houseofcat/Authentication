USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[Role_Update]
GO

CREATE PROCEDURE [Identity].[Role_Update]
(
    @RoleId INT,
    @Name NVARCHAR(256),
    @NormalizedName NVARCHAR(256),
    @CurrencyStamp NVARCHAR(MAX),
    @RoleName NVARCHAR(256)
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    UPDATE
        [Identity].[Roles]
    SET
        [Name] = @Name,
        [NormalizedName] = @NormalizedName,
        [ConcurrencyStamp] = @CurrencyStamp,
        [RoleName] = @RoleName
    WHERE
        RoleId = @RoleId

END
GO
