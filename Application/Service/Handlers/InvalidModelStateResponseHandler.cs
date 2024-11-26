using System.Net;

using Microsoft.AspNetCore.Mvc;

using Service.Models.Errors;
using Service.ObjectResults;

namespace Service.Handlers
{
    public static class InvalidModelStateResponseHandler
    {
        public static IActionResult GetErrorResponse(ActionContext context)
        {
            IEnumerable<string> keys = context.ModelState.Keys.Select(key => key);
            ValidationError[] errors = keys.SelectMany(key => context.ModelState[key]!.Errors.Select(error => new ValidationError(key, error.ErrorMessage))).ToArray();

            Models.ErrorContext errorContext = new Models.ErrorContext()
            {
                Error = new ErrorCollection()
                {
                    Errors = errors
                }
            };
            
            return new ActionObjectResult(errorContext, (int)HttpStatusCode.BadRequest);
        }
    }
}
