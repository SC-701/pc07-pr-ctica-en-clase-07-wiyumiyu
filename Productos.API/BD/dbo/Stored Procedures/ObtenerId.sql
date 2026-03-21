-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ObtenerId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id,
        p.IdSubCategoria,
        p.Nombre,
        p.Descripcion,
        p.Precio,
        p.Stock,
        p.CodigoBarras,
        c.Nombre AS Categoria,
        s.Nombre AS SubCategoria
    FROM Producto p
    LEFT JOIN SubCategorias s
        ON p.IdSubCategoria = s.Id
    LEFT JOIN Categorias c
        ON s.IdCategoria = c.Id
    WHERE p.Id = @Id;
END;
GO
