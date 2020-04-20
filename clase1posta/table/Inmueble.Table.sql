CREATE TABLE [dbo].[Inmuebles]
(
	[IdInmueble] INT          IDENTITY (10, 1) NOT NULL,
    [IdPropietario] VARCHAR(50) NULL, 
    [Direccion] VARCHAR(50) NULL, 
    [Tipo] VARCHAR(50) NULL, 
    [CantAmbientes] VARCHAR(50) NULL, 
    [Precio] VARCHAR(50) NULL, 
    [Estado] TINYINT NULL, 
    CONSTRAINT [PK_Inmuebles] PRIMARY KEY ([IdInmueble])
)
