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
        }
    }

}
