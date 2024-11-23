using Microsoft.AspNetCore.Mvc;
using Service.Models.Errors;
using Service.Validation;
using System.Net;

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
            
            return new ValidationFailedResult(errorContext, (int)HttpStatusCode.BadRequest);
        }
    }
}
