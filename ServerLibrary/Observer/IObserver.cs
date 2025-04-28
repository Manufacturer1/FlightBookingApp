namespace ServerLibrary.Observer
{
    public interface IObserver<T>
    {
        Task Notify(T data, string subject, string message);
    }
}
