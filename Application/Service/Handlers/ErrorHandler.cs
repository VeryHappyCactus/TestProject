using System.Net;
using Common.Enums.Errors;
using Common.Settings;

using MediatorHandlers.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Service.Models.Errors;

namespace Service.Handlers
{
    public class ErrorHandler : IExceptionHandler
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;
        private readonly IAppCommonSettings _appCommonSettings;

        public ErrorHandler(IAppCommonSettings appCommonSettings, RequestDelegate next, ILogger<ErrorHandler> logger)
        {
            _next = next;
            _logger = logger;
            _appCommonSettings = appCommonSettings;
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

                await httpContext.Response.WriteAsJsonAsync(errorContext, _appCommonSettings.JsonSettings!.JsonSerializerOption!);

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

                await httpContext.Response.WriteAsJsonAsync(errorContext, _appCommonSettings.JsonSettings!.JsonSerializerOption!);
            }

            return false;
        }
    }
}
