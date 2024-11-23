using Microsoft.AspNetCore.Mvc;

namespace Service.Validation
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(object? obj, int? statusCode)
            : base(obj)
        {
            StatusCode = statusCode;
        }
    }
}
