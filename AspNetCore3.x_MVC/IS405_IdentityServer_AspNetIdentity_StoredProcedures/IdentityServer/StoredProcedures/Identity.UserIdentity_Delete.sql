USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserIdentity_Delete]
GO

CREATE PROCEDURE [Identity].[UserIdentity_Delete]
(
    @UserId BIGINT
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DELETE FROM [Identity].[UserIdentities]
    WHERE [UserId] = @UserId

END
GO
