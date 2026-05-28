namespace BE
{
    public class Usuario : Entidad
    {
        private string Username {  get; set; }
        private string Passwordhash { get; set; }

        public Usuario(string nUsername, string nPasswordHash)
        {
            Username = nUsername;
            Passwordhash = nPasswordHash;
        }
    }
}
