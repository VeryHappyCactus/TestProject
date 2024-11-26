
namespace ServiceLogic.Exceptions
{
    public class HandlerException : Exception
    {
        public string ErrorClass { get; set; }
        public int ErrorCode { get; set; }
        public HandlerErrorTypes ErrorType { get; set; }

        public HandlerException(string errorClass, int errorCode, string message, HandlerErrorTypes errorType)
            : base(message)
        {
            ErrorClass = errorClass;
            ErrorCode = errorCode;
            ErrorType = errorType;
        }
    }
}
