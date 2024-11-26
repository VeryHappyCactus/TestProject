using MediatR;

namespace MediatorHandlers.Handlers.CommonModels.Request
{
    public class DefaultRequest : IRequest<object?>
    {
        public Type RequestType { get; set; }
        public Type ResultType { get; set; }
        public object Model { get; set; }
        public string Operation { get; set; }

        public DefaultRequest(object model, Type requestType, Type resultType, string operation)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (requestType == null)
                throw new ArgumentNullException(nameof(requestType));

            if (resultType == null)
                throw new ArgumentNullException(nameof(resultType));

            if (string.IsNullOrEmpty(operation))
                throw new ArgumentNullException(nameof(operation));
            
            Model = model;
            RequestType = requestType;
            ResultType = resultType;
            Operation = operation;
        }
    }
}
