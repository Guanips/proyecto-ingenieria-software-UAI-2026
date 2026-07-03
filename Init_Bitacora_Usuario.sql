CREATE TABLE Usuario (
    ID UNIQUEIDENTIFIER PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    NumTelefono NVARCHAR(20) NOT NULL,
    EstaBloqueado BIT NOT NULL,
    Idioma VARCHAR(5) NOT NULL DEFAULT 'ES',
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

INSERT INTO Usuario VALUES (
	'd1eda407-3582-4e0c-85cc-ae51eb67b826',
	'admin',
	'8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918',
	'admin@gmail.com',
	'+54 1120202020',
	0,
	DEFAULT,
	DEFAULT
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
-------------


-- 2. TABLAS DE IDIOMAS Y TRADUCCIONES
CREATE TABLE Idioma (
    Codigo VARCHAR(5) NOT NULL,   
    Nombre VARCHAR(50) NOT NULL,  
    CONSTRAINT PK_Idioma PRIMARY KEY (Codigo)
);

CREATE TABLE Traduccion (
    IdTraduccion INT IDENTITY(1,1) NOT NULL,
    CodigoIdioma VARCHAR(5) NOT NULL,
    KeyEtiqueta VARCHAR(100) NOT NULL, 
    Texto NVARCHAR(MAX) NOT NULL,      
    CONSTRAINT PK_Traduccion PRIMARY KEY (IdTraduccion),
    CONSTRAINT FK_Traduccion_Idioma FOREIGN KEY (CodigoIdioma) REFERENCES Idioma(Codigo)
);
CREATE UNIQUE INDEX UIX_Idioma_Etiqueta ON Traduccion(CodigoIdioma, KeyEtiqueta);


-- ---------------------------------------------------------
-- INSERTS INICIALES

-- ---------------------------------------------------------
-- 1. REGISTRAR LOS IDIOMAS
-- ---------------------------------------------------------
INSERT INTO Idioma (Codigo, Nombre) VALUES ('ES', 'Español');
INSERT INTO Idioma (Codigo, Nombre) VALUES ('EN', 'English');
INSERT INTO Idioma (Codigo, Nombre) VALUES ('PT', 'Português');

-- ---------------------------------------------------------
------------------------------------------Separados por Idioma
-- =========================================================================
-- 1. TRADUCCIONES AL ESPAÑOL (ES)
-- =========================================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'MainUI', 'Sistema de gestion');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemCerrarSesion', 'Cerrar sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemIniciarSesion', 'Iniciar sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemGestionDeUsuarios', 'Gestión de usuarios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemABMUsuarios', 'ABM Usuarios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Desploqueo de usuarios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemGestionDePerfiles', 'Gestión de perfiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemABMPerfiles', 'Alta y asignación de perfiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemInicio', 'Inicio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemBitacora', 'Bitacora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemConsultarBitacora', 'Consultar bitacora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemPerfiles', 'Perfiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemGestionarPerfiles', 'Gestionar perfiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'label1', 'Idioma');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Registrar usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelUsername', 'Username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelEmail', 'Email');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Número de telefono');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelContrasena', 'Contraseña');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repetir Contraseña');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirmar registro');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'Listado de usuarios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modificar usuario seleccionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Eliminar usuario seleccionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIModificacionLabelEmail', 'Email');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Número de telefono');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirmar modificación');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'loginUILabelUsername', 'Usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'loginUILabelContrasena', 'Contraseña');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'loginUIButtonIniciarSesion', 'Iniciar sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIGroupBoxTreeView', 'Arbol de perfiles y permisos');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUILabelNombrePerfil', 'Nombre del nuevo perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIButtonCrearPerfil', 'Crear perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIGroupBoxListBoxPerfiles', 'Perfiles disponibles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilUIButtonAsignarPerfil', 'Asignar perfil a perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIGroupBoxUsuarios', 'Usuarios disponibles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilUIButtonAsignarPerfilUsuario', 'Asignar perfil a usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilUIButtonDesasignarPerfilUsuario', 'Desasignar perfil a usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIGroupBoxListBoxPermisos', 'Permisos disponibles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilUIButtonAsignarPermiso', 'Asignar permiso a perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'bitacoraUILabelGrid', 'Registros de la bitacora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'bitacoraUILabelComboBoxAccion', 'Filtrado por acción');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'bitacoraUILabelComboBoxUsername', 'Filtrado por username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'bitacoraUIButtonLimpiarFiltros', 'Limpiar filtros');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_InicioSesionExito', 'Inicio de sesión exitoso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_TituloExito', 'Éxito');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_CierreSesionExito', 'Sesión cerrada correctamente.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_TituloCierreSesion', 'Cerrar sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_MaxIntentos', 'Ha superado los 3 intentos fallidos. Su cuenta ha sido bloqueada por seguridad.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_QuedanIntentos', 'Contraseña incorrecta. Le quedan {0} intentos antes de bloquearse.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_NoUserLogout', 'Usuario activo no encontrado en logout.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'log_InicioSesion', 'Inicio de Sesion');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'log_CierreSesion', 'Cierre de Sesion');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_UsuarioIncorrecto', 'Usuario incorrecto');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_UsuarioBloqueado', 'El usuario se encuentra bloqueado.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_EmailVacio', 'El correo electrónico no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_EmailFormato', 'El formato del correo electrónico no es válido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_UsernameVacio', 'El nombre de usuario no puede estar vacio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_UsernameFormato', 'El nombre de usuario debe tener entre 3 y 16 caracteres y solo puede contener letras, números, guiones bajos y guiones');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_PhoneVacio', 'El número de teléfono no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_PhoneFormato', 'El formato del número de teléfono no es válido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_OnlyLettersVacio', 'El campo de texto no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_OnlyLettersFormato', 'El campo solo puede contener letras y espacios (se permiten acentos y eñes)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_AlphaNumStrictVacio', 'El código o ID no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_AlphaNumStrictFormato', 'El campo solo puede contener letras (sin acentos) y números, sin espacios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_AlphaNumSpacesVacio', 'El texto no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_AlphaNumSpacesFormato', 'El campo solo puede contener letras, números y espacios (sin caracteres especiales)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_PassVacia', 'La contraseña no puede estar vacía.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_PassNoCoincide', 'Las contraseñas no coinciden.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_NoUserModificar', 'No se ha seleccionado ningún usuario para modificar.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_NoUserEliminar', 'No se ha seleccionado ningún usuario para eliminar.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_TituloError', 'Error');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btnDesbloquear', 'Desbloquear usuario seleccionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_NoUserDesbloquear', 'No se ha seleccionado ningún usuario para desbloquear.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_DesbloqueoExito', 'Usuario desbloqueado correctamente.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_LOGIN', 'Inició sesión en el sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_LOGOUT', 'Cerró sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_USER_ADD', 'Registró a un nuevo usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_USER_MOD', 'Modificó los datos de un usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_USER_DEL', 'Eliminó a un usuario del sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_PERFIL_ADD', 'Asignó un perfil a un usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_PERMISOS_MOD', 'Modificó permisos del sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridBitacora_Usuario', 'Usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridBitacora_Fecha', 'Fecha y Hora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridBitacora_Accion', 'Acción Realizada');

-- =========================================================================
-- 2. TRADUCCIONES AL INGLÉS (EN)
-- =========================================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'MainUI', 'Management System');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemCerrarSesion', 'Logout');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemIniciarSesion', 'Login');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemGestionDeUsuarios', 'User Management');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemABMUsuarios', 'CRUD Users');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Unlock Users');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemGestionDePerfiles', 'Profile Management');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemABMPerfiles', 'Profile Creation & Assignment');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemInicio', 'Home');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemBitacora', 'Logbook');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemConsultarBitacora', 'View Logbook');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemPerfiles', 'Profiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemGestionarPerfiles', 'Manage Profiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'label1', 'Language');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Register User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelUsername', 'Username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelEmail', 'Email');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Phone Number');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelContrasena', 'Password');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repeat Password');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirm Registration');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'User List');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modify Selected User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Delete Selected User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIModificacionLabelEmail', 'Email');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Phone Number');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirm Modification');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'loginUILabelUsername', 'Username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'loginUILabelContrasena', 'Password');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'loginUIButtonIniciarSesion', 'Login');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIGroupBoxTreeView', 'Profiles and Permissions Tree');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUILabelNombrePerfil', 'New Profile Name');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIButtonCrearPerfil', 'Create Profile');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIGroupBoxListBoxPerfiles', 'Available Profiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilUIButtonAsignarPerfil', 'Assign Profile to Profile');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIGroupBoxUsuarios', 'Available Users');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilUIButtonAsignarPerfilUsuario', 'Assign Profile to User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilUIButtonDesasignarPerfilUsuario', 'Unassign Profile from User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIGroupBoxListBoxPermisos', 'Available Permissions');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilUIButtonAsignarPermiso', 'Assign Permission to Profile');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'bitacoraUILabelGrid', 'Binnacle entries');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'bitacoraUILabelComboBoxAccion', 'Filter by action');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'bitacoraUILabelComboBoxUsername', 'Filter by username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'bitacoraUIButtonLimpiarFiltros', 'Clean filters');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_InicioSesionExito', 'Successful login.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_TituloExito', 'Success');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_CierreSesionExito', 'Session closed successfully.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_TituloCierreSesion', 'Logout');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_MaxIntentos', 'Maximum failed attempts exceeded. Your account has been locked for security.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_QuedanIntentos', 'Incorrect password. You have {0} attempts left before being locked.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_NoUserLogout', 'Active user not found on logout.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'log_InicioSesion', 'Login');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'log_CierreSesion', 'Logout');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_UsuarioIncorrecto', 'Incorrect user');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_UsuarioBloqueado', 'The user is locked.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_EmailVacio', 'Email cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_EmailFormato', 'Invalid email format');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_UsernameVacio', 'Username cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_UsernameFormato', 'Username must be between 3 and 16 characters and can only contain letters, numbers, underscores, and hyphens');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_PhoneVacio', 'Phone number cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_PhoneFormato', 'Invalid phone number format');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_OnlyLettersVacio', 'The text field cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_OnlyLettersFormato', 'The field can only contain letters and spaces (accents and ñ are allowed)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_AlphaNumStrictVacio', 'Code or ID cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_AlphaNumStrictFormato', 'The field can only contain letters (no accents) and numbers, without spaces');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_AlphaNumSpacesVacio', 'Text cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_AlphaNumSpacesFormato', 'The field can only contain letters, numbers, and spaces (no special characters)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_PassVacia', 'Password cannot be empty.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_PassNoCoincide', 'Passwords do not match.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_NoUserModificar', 'No user selected to modify.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_NoUserEliminar', 'No user selected to delete.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_TituloError', 'Error');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btnDesbloquear', 'Unlock selected user');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_NoUserDesbloquear', 'No user selected to unlock.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_DesbloqueoExito', 'User unlocked successfully.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_LOGIN', 'Logged into the system');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_LOGOUT', 'Logged out');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_USER_ADD', 'Registered a new user');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_USER_MOD', 'Modified user details');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_USER_DEL', 'Deleted a user from the system');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_PERFIL_ADD', 'Assigned a profile to a user');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_PERMISOS_MOD', 'Modified system permissions');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridBitacora_Usuario', 'User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridBitacora_Fecha', 'Date and Time');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridBitacora_Accion', 'Action Performed');

-- =========================================================================
-- 3. TRADUCCIONES AL PORTUGUÉS (PT)
-- =========================================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'MainUI', 'Sistema de Gestão');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemCerrarSesion', 'Sair');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemIniciarSesion', 'Entrar');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemGestionDeUsuarios', 'Gestão de Usuários');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemABMUsuarios', 'CRUD de Usuários');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Desbloquear Usuários');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemGestionDePerfiles', 'Gestão de Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemABMPerfiles', 'Criação e Atribuição de Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemInicio', 'Início');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemBitacora', 'Livro de Bordo');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemConsultarBitacora', 'Visualizar Livro de Bordo');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemPerfiles', 'Perfis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemGestionarPerfiles', 'Gerenciar Perfis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'label1', 'Idioma');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Registrar Usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelUsername', 'Nome de usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelEmail', 'E-mail');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Número de Telefone');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelContrasena', 'Senha');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repetir Senha');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirmar Registro');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'Lista de Usuários');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modificar Usuário Selecionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Excluir Usuário Selecionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIModificacionLabelEmail', 'E-mail');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Número de Telefone');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirmar Modificação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'loginUILabelUsername', 'Nome de usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'loginUILabelContrasena', 'Senha');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'loginUIButtonIniciarSesion', 'Entrar');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIGroupBoxTreeView', 'Árvore de Perfis e Permissões');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUILabelNombrePerfil', 'Nome do Novo Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIButtonCrearPerfil', 'Criar Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIGroupBoxListBoxPerfiles', 'Perfis Disponíveis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilUIButtonAsignarPerfil', 'Atribuir Perfil a Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIGroupBoxUsuarios', 'Usuários Disponíveis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilUIButtonAsignarPerfilUsuario', 'Atribuir Perfil a Usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilUIButtonDesasignarPerfilUsuario', 'Remover Perfil do Usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIGroupBoxListBoxPermisos', 'Permissões Disponíveis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilUIButtonAsignarPermiso', 'Atribuir Permissão ao Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'bitacoraUILabelGrid', 'Registros de Borda');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'bitacoraUILabelComboBoxAccion', 'Filtrar por ação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'bitacoraUILabelComboBoxUsername', 'Filtrar por nome de usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'bitacoraUIButtonLimpiarFiltros', 'Limpar filtros');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_InicioSesionExito', 'Login bem-sucedido.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_TituloExito', 'Sucesso');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_CierreSesionExito', 'Sessão encerrada com sucesso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_TituloCierreSesion', 'Sair');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_MaxIntentos', 'Limite de tentativas excedido. Sua conta foi bloqueada por segurança.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_QuedanIntentos', 'Senha incorreta. Você tem {0} tentativas restantes antes de ser bloqueado.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_NoUserLogout', 'Usuário ativo não encontrado no logout.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'log_InicioSesion', 'Início de Sessão');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'log_CierreSesion', 'Encerramento de Sessão');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_UsuarioIncorrecto', 'Usuário incorreto');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_UsuarioBloqueado', 'O usuário está bloqueado.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_EmailVacio', 'O e-mail não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_EmailFormato', 'Formato de e-mail inválido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_UsernameVacio', 'O nome de usuário não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_UsernameFormato', 'O nome de usuário deve ter entre 3 e 16 caracteres e só pode conter letras, números, sublinhados e hifens');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_PhoneVacio', 'O número de telefone não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_PhoneFormato', 'Formato de número de telefone inválido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_OnlyLettersVacio', 'O campo de texto não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_OnlyLettersFormato', 'O campo só pode conter letras e espaços (acentos e cedilhas são permitidos)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_AlphaNumStrictVacio', 'O código ou ID não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_AlphaNumStrictFormato', 'O campo só pode conter letras (sem acentos) e números, sem espaços');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_AlphaNumSpacesVacio', 'O texto não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_AlphaNumSpacesFormato', 'O campo só pode conter letras, números e espaços (sem caracteres especiais)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_PassVacia', 'A senha não pode estar vazia.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_PassNoCoincide', 'As senhas não coincidem.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_NoUserModificar', 'Nenhum usuário selecionado para modificar.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_NoUserEliminar', 'Nenhum usuário selecionado para excluir.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_TituloError', 'Erro');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btnDesbloquear', 'Desbloquear usuário selecionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_NoUserDesbloquear', 'Nenhum usuário selecionado para desbloquear.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_DesbloqueoExito', 'Usuário desbloqueado com sucesso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_LOGIN', 'Entrou no sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_LOGOUT', 'Saiu do sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_USER_ADD', 'Registrou um novo usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_USER_MOD', 'Modificou os dados de um usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_USER_DEL', 'Excluiu um usuário do sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_PERFIL_ADD', 'Atribuiu um perfil a um usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_PERMISOS_MOD', 'Modificou as permissões do sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridBitacora_Usuario', 'Usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridBitacora_Fecha', 'Data e Hora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridBitacora_Accion', 'Ação Realizada');

