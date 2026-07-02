namespace BE
{
    public interface IObserver
    {
        public void Update(Usuario usuarioInvolucrado, string action);
    }
}
