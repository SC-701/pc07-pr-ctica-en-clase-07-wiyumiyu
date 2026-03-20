-- =============================================
-- Author:		TuNombre
-- Create date: GETDATE()
-- Description:	Obtiene todas las categorías
-- =============================================
CREATE PROCEDURE ObtenerCategorias
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Nombre
	FROM Categorias
END