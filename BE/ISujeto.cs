namespace BE
{
    public interface ISujeto
    {
        public void Attach(IObserver observer);
        public void Detach(IObserver observer);
        public void Notificar(string username, string accion);
    }
}
