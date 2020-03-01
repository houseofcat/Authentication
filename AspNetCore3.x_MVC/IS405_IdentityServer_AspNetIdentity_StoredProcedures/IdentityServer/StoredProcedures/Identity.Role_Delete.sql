USE [Identity]
GO

DROP PROCEDURE IF EXISTS [Identity].[Role_Delete]
GO

CREATE PROCEDURE [Identity].[Role_Delete]
(
    @RoleId INT
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DELETE FROM [Identity].[Roles]
    WHERE [RoleId] = @RoleId

END
GO

