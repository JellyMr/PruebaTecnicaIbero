CREATE DATABASE CatalogoProductos;
GO

USE CatalogoProductos;
GO

CREATE TABLE Producto
(
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    Descripcion VARCHAR(500) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Existencia INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE PROCEDURE SP_Productos_Seleccionar
AS
BEGIN

    SELECT
        IdProducto,
        Nombre,
        Descripcion,
        Precio,
        Existencia,
        Activo,
        FechaRegistro
    FROM Producto
    WHERE Activo = 1
    ORDER BY IdProducto DESC;

END;
GO

CREATE PROCEDURE SP_Productos_Insertar
    @Nombre VARCHAR(150),
    @Descripcion VARCHAR(500),
    @Precio DECIMAL(10,2),
    @Existencia INT
AS
BEGIN



    INSERT INTO Producto
    (
        Nombre,
        Descripcion,
        Precio,
        Existencia
    )
    VALUES
    (
        @Nombre,
        @Descripcion,
        @Precio,
        @Existencia
    );

END;
GO

CREATE PROCEDURE SP_Productos_Eliminar
    @IdProducto INT
AS
BEGIN

    UPDATE Producto
    SET Activo = 0
    WHERE IdProducto = @IdProducto;

END;
GO
