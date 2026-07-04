namespace BE
{
    public class Usuario : Entidad
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string NumTelefono { get; private set; }
        public string Email { get; private set; }
        public bool EstaBloqueado { get; private set; }
        public string Idioma { get; set; }
        public int IntentosFallidos { get; private set; }
        public List<Permiso> Permisos { get; private set; }

        public Usuario(Guid id, string nUsername, string nPasswordHash, string nEmail, string nNumTelefono, bool estaBloqueado, string idioma, int nIntentosFallidos)
        {
            Id = id;
            Username = nUsername;
            PasswordHash = nPasswordHash;
            Email = nEmail;
            NumTelefono = nNumTelefono;
            EstaBloqueado = estaBloqueado;
            Idioma = idioma;
            IntentosFallidos = nIntentosFallidos;
            Permisos = new List<Permiso>();
        }

        public void ModificarEstado(string nuevoEmail, string nuevoNumTelefono)
        {
            Email = nuevoEmail;
            NumTelefono = nuevoNumTelefono;
        }

        public void RestaurarEstado(IMementoUsuario memento)
        {
            if (memento is not IEstadoUsuario estado)
            {
                throw new ArgumentException("El memento no contiene un estado válido para esta entidad.");
            }

            Email = estado.Email;
            NumTelefono = estado.NumTelefono;
        }

        public IMementoUsuario GuardarEstado(string descripcion)
        {
            return new MementoUsuario(Email, NumTelefono, DateTime.Now, descripcion);
        }

        private class MementoUsuario : IEstadoUsuario
        {
            public string Email { get; }
            public string NumTelefono { get; }
            public DateTime Fecha { get; }
            public string Descripcion { get; }

            public MementoUsuario(string email, string numTelefono, DateTime fecha, string descripcion)
            {
                Email = email;
                NumTelefono = numTelefono;
                Fecha = fecha;
                Descripcion = descripcion;
            }
        }
    }

}
