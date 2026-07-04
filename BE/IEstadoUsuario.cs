namespace BE
{
    public interface IEstadoUsuario : IMementoUsuario
    {
        string Email { get; }
        string NumTelefono { get; }
    }
}
