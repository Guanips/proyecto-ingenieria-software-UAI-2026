namespace BE
{
    public class MementoUsuario : Entidad
    {
        private string Email { get; set; }
        private string NumTelefono { get; set; }

        public MementoUsuario(string nEmail, string nNumTelefono)
        {
            Email = nEmail;
            NumTelefono = nNumTelefono;
        }

        public string GetEmail()
        {
            return Email;
        }

        public string GetNumTelefono()
        {
            return NumTelefono;
        }
    }
}
