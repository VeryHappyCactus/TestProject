using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediatorHandlers.Exceptions
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
