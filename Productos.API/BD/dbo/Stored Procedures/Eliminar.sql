-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE   PROCEDURE Eliminar
(
    @Id UNIQUEIDENTIFIER
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    DELETE
    FROM dbo.Producto
    WHERE Id = @Id;
    SELECT @Id
    COMMIT TRANSACTION
END