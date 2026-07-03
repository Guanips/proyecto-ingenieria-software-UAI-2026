namespace BE
{
    public interface IObserver
    {
        public abstract void Update(string username, string action);
    }
}
