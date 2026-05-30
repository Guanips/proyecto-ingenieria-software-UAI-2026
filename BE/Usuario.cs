namespace BE
{
    public class Usuario : Entidad
    {
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }

        public Usuario() { }
        public Usuario(object[] ob) : this((Guid)ob[0], (string)ob[1], (string)ob[2] ) { }

        public Usuario(Guid id,string nUsername, string nPasswordHash) //Por ahora hardcodeamos el Usuario porque solo sera uno del tipo administrador
        {
            Id = id;
            Username = nUsername;
            PasswordHash = nPasswordHash;
        }
    }

}
