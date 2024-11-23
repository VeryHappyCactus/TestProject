using System.Text.Json.Serialization;
using Service.Models.Errors;

namespace Service.Models
{

    [JsonDerivedType(typeof(OperationError), nameof(OperationError))]
    [JsonDerivedType(typeof(ValidationError), nameof(ValidationError))]
    [JsonDerivedType(typeof(ErrorCollection), nameof(ErrorCollection))]
    public class BaseError
    {
    }
}
