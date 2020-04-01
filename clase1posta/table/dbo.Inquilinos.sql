CREATE TABLE [dbo].[Inquilinos]
(
	[IdInquilino] INT NOT NULL PRIMARY KEY, 
    [Nombre] VARCHAR(50) NULL, 
    [Apellido] VARCHAR(50) NULL, 
    [Trabajo] VARCHAR(50) NULL, 
    [NombreGarante] VARCHAR(50) NULL, 
    [ApellidoGarante] VARCHAR(50) NULL
)
