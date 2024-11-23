namespace Service.Models.Errors
{
    public class OperationError : BaseError
    {
        public string? ErrorClass { get; set; }
        public int ErrorCode { get; set; }
        public string? Message { get; set; }

        public OperationError(string errorClass, int errorCode, string message) 
        { 
            ErrorClass = errorClass;
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
