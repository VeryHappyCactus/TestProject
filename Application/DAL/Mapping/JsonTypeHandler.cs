using System.Data;
using System.Text.Json;

using Dapper;

namespace DAL.Mapping
{
    public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
        where T : class
    {
        private readonly JsonSerializerOptions _jsonSerializerOption;

        public JsonTypeHandler(JsonSerializerOptions jsonSerializerOption) 
        { 
            if (jsonSerializerOption == null)
                throw new ArgumentNullException(nameof(jsonSerializerOption));

            _jsonSerializerOption = jsonSerializerOption;
        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            //parameter.Value = JsonSerializer.Serialize(value)!;
            throw new NotImplementedException();
        }

        public override T? Parse(object value)
        {
            if (value is string json)
            {
                return JsonSerializer.Deserialize<T>(json, _jsonSerializerOption);
            }

            return null;
        }
    }
}
