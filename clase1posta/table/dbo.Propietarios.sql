CREATE TABLE [dbo].[Propietarios] (
    [IdPropietario]     INT          IDENTITY (10, 1) NOT NULL,
    [Nombre] VARCHAR (50) NULL,
    [Apellido]  VARCHAR (50) NULL,
    [Dni] VARCHAR(50) NULL, 
    [Telefono] VARCHAR(50) NULL, 
    [Email] VARCHAR(50) NULL, 
    PRIMARY KEY CLUSTERED ([IdPropietario] ASC)
);

