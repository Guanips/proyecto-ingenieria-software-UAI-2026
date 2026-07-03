namespace BE
{
    public interface ISujeto
    {
        public abstract void Attach(IObserver observer);
        public abstract void Detach(IObserver observer);
        public abstract void Notificar(string username, string accion);
    }
}
