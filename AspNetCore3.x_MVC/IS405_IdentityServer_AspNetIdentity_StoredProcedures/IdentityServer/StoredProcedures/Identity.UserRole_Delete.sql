USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[UserRole_Delete]
GO

CREATE PROCEDURE [Identity].[UserRole_Delete]
(
    @Id BIGINT
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DELETE FROM [Identity].[UserRoles]
    WHERE [Id] = @Id

END
GO

