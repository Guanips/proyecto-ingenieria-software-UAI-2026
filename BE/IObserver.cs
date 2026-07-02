namespace BE
{
    public interface IObserver
    {
        public abstract void Update(Usuario usuarioInvolucrado, string action);
    }
}
