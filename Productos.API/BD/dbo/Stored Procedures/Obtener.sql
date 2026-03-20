-- =============================================
    -- Author:		<Author,,Name>
    -- Create date: <Create Date,,>
    -- Description:	<Description,,>
    -- =============================================
    --CREATE   PROCEDURE Obtener
    --AS
    --BEGIN
    --    SET NOCOUNT ON;
    --SELECT Categorias.*, Producto.*, SubCategorias.*
    --FROM   Categorias INNER JOIN
    --             Producto ON Categorias.Id = Producto.Id INNER JOIN
    --             SubCategorias ON Categorias.Id = SubCategorias.IdCategoria AND Producto.IdSubCategoria = SubCategorias.Id

    --END

CREATE PROCEDURE Obtener
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id,
        p.Nombre,
        p.Descripcion,
        p.Precio,
        p.Stock,
        p.CodigoBarras,

        s.Nombre AS SubCategoria,
        c.Nombre AS Categoria

    FROM Producto p

    LEFT JOIN SubCategorias s
        ON p.IdSubCategoria = s.Id

    LEFT JOIN Categorias c
        ON s.IdCategoria = c.Id
END;
GO