CREATE TABLE Usuario (
	ID UNIQUEIDENTIFIER PRIMARY KEY,
	Username NVARCHAR(50) NOT NULL UNIQUE,
	PasswordHash NVARCHAR(100) NOT NULL,
	Email NVARCHAR(100) NOT NULL,
	NumTelefono NVARCHAR(20) NOT NULL,
	EstaBloqueado BIT NOT NULL,
	Idioma NVARCHAR(20) NOT NULL,
	IntentosFallidos INT NOT NULL DEFAULT 0
);

CREATE TABLE Bitacora (
	ID INT PRIMARY KEY IDENTITY(0,1),
	Username NVARCHAR(50) NOT NULL,
	Fecha DATETIME NOT NULL,
	Accion NVARCHAR(50) NOT NULL
);

CREATE TABLE Permiso (
    ID INT PRIMARY KEY IDENTITY(0,1),
    Nombre VARCHAR(30) NOT NULL UNIQUE,
    EsPerfil BIT NOT NULL
);

CREATE TABLE PermisoRelacion (
    ID_Padre INT NOT NULL,
    ID_Hijo INT NOT NULL,
    PRIMARY KEY (ID_Padre, ID_Hijo),
    CONSTRAINT FK_PermisoRelacion_Padre FOREIGN KEY (ID_Padre) REFERENCES Permiso(ID),
    CONSTRAINT FK_PermisoRelacion_Hijo FOREIGN KEY (ID_Hijo) REFERENCES Permiso(ID)
);


CREATE TABLE PerfilUsuario (
	ID_Usuario UNIQUEIDENTIFIER,
	ID_Perfil INT,
	PRIMARY KEY (ID_Usuario, ID_Perfil),
	CONSTRAINT FK_PerfilUsuarioUsuario FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID),
	CONSTRAINT FK_PerfilUsuarioPerfil FOREIGN KEY (ID_Perfil) REFERENCES Permiso(ID)
);

CREATE TABLE Idioma (
    Codigo VARCHAR(5) NOT NULL,   -- Ej: 'ES', 'EN'
    Nombre VARCHAR(50) NOT NULL,  -- Ej: 'Español', 'English'
    CONSTRAINT PK_Idioma PRIMARY KEY (Codigo)
);

CREATE TABLE Traduccion (
    IdTraduccion INT IDENTITY(1,1) NOT NULL,
    CodigoIdioma VARCHAR(5) NOT NULL,
    KeyEtiqueta VARCHAR(100) NOT NULL, -- El nombre del control (Ej: loginUILabelUsername)
    Texto NVARCHAR(MAX) NOT NULL,      -- El texto a mostrar (Ej: 'Nombre de usuario')
    CONSTRAINT PK_Traduccion PRIMARY KEY (IdTraduccion),
    CONSTRAINT FK_Traduccion_Idioma FOREIGN KEY (CodigoIdioma) REFERENCES Idioma(Codigo)
);

CREATE UNIQUE INDEX UIX_Idioma_Etiqueta ON Traduccion(CodigoIdioma, KeyEtiqueta);


INSERT INTO Usuario VALUES (
	'd1eda407-3582-4e0c-85cc-ae51eb67b826',
	'admin',
	'8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918',
	'admin@gmail.com',
	'+54 1120202020',
	0,
	'ES',
	0
);

INSERT INTO Permiso VALUES
('PERM-GESTIONAR-USR', 0),
('PERM-GESTIONAR-IDM', 0),
('PERM-DESBLOQUEAR-USR', 0),
('PERM-GESTIONAR-PERFIL', 0),
('PERM-CONSULTA-BIT', 0),
('PERF-ADMIN', 1);

INSERT INTO PermisoRelacion VALUES
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GESTIONAR-USR' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GESTIONAR-IDM' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GESTIONAR-PERFIL' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-CONSULTA-BIT' AND EsPerfil = 0));

INSERT INTO PerfilUsuario VALUES ('d1eda407-3582-4e0c-85cc-ae51eb67b826', (SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1));