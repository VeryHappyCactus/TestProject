using System.Text.Json.Serialization;

using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Message.ClientOperation.Result;

namespace Common.Queue.Message
{

    [JsonDerivedType(typeof(MessageCollection), nameof(MessageCollection))]
    [JsonDerivedType(typeof(ErrorMessage), nameof(ErrorMessage))]
    [JsonDerivedType(typeof(ClientOperationRequestMessage), nameof(ClientOperationRequestMessage))]
    [JsonDerivedType(typeof(ClientOperationsRequestMessage), nameof(ClientOperationsRequestMessage))]
    [JsonDerivedType(typeof(ClientOperationResultMessage), nameof(ClientOperationResultMessage))]
    [JsonDerivedType(typeof(ClientWithdrawOperationResultMessage), nameof(ClientWithdrawOperationResultMessage))]
    [JsonDerivedType(typeof(ClientWithdrawOperationRequestMessage), nameof(ClientWithdrawOperationRequestMessage))]
    [JsonDerivedType(typeof(ExchangeCourseRequestMessage), nameof(ExchangeCourseRequestMessage))]
    [JsonDerivedType(typeof(ExchangeCourseResultMessage), nameof(ExchangeCourseResultMessage))]
    public class BaseMessage
    {
    }
}
