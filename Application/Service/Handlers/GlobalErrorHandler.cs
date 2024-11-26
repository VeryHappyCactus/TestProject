using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

using ServiceLogic.Exceptions;
using Service.Models.Errors;
using Common.Enums.Errors;

namespace Service.Handlers
{
    public class GlobalErrorHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalErrorHandler> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public GlobalErrorHandler(JsonSerializerOptions jsonSerializerOptions, ILogger<GlobalErrorHandler> logger)
        {
            if (jsonSerializerOptions == null)
                throw new ArgumentNullException(nameof(jsonSerializerOptions));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            if (exception is HandlerException ex)
            {
                _logger.LogError(ex.Message, ex);


                if (ex.ErrorType == HandlerErrorTypes.RequestError)
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                else if (ex.ErrorType == HandlerErrorTypes.Internal)
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                Models.ErrorContext errorContext = new Models.ErrorContext()
                {
                    Error = new OperationError(ex.ErrorClass, ex.ErrorCode, ex.Message)
                };

                await httpContext.Response.WriteAsJsonAsync(errorContext, _jsonSerializerOptions);

                return true;
            }
            else
            {
                _logger.LogError(exception.Message, exception);

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                Models.ErrorContext errorContext = new Models.ErrorContext()
                {
                    Error = new OperationError(nameof(CommonErrorTypes), (int)CommonErrorTypes.Internal, "Internal server error")
                };

                await httpContext.Response.WriteAsJsonAsync(errorContext, _jsonSerializerOptions);
            }

            return false;
        }
    }
}
