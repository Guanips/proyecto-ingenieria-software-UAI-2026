namespace BE
{
    public interface IObserver
    {
        public void Update(string username, string action);
    }
}
