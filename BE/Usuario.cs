namespace BE
{
    public class Usuario : Entidad
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public Usuario(Guid id,string nUsername, string nPasswordHash)
        {
            Id = id;
            Username = nUsername;
            PasswordHash = nPasswordHash;
        }
    }

}
