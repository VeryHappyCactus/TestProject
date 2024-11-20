
namespace MediatorHandlers.Handlers
{
    public class HandlerResultContext<T>
    {
        public T? Value { get; set; }
        public HandlerErrorContext? Error { get; set; }
    }
}
