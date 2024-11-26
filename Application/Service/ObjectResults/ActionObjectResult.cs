using Microsoft.AspNetCore.Mvc;

namespace Service.ObjectResults
{
    public class ActionObjectResult : ObjectResult
    {
        public ActionObjectResult(object? obj, int? statusCode)
            : base(obj)
        {
            StatusCode = statusCode;
        }
    }
}
