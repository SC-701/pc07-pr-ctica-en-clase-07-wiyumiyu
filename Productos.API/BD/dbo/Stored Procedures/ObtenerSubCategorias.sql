-- =============================================
-- Author:		TuNombre
-- Create date: GETDATE()
-- Description:	Obtiene subcategorías por categoría
-- =============================================
CREATE PROCEDURE ObtenerSubCategorias
	@IdCategoria UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, IdCategoria, Nombre
	FROM SubCategorias
	WHERE IdCategoria = @IdCategoria
END