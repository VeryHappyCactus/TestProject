using System.Data;
using System.Text.Json;
using Common.Settings;
using Dapper;

namespace DAL.Mapping
{
    public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
        where T : class
    {
        private readonly IAppCommonSettings _appCommonSettings;

        public JsonTypeHandler(IAppCommonSettings appCommonSettings) 
        { 
            if (appCommonSettings == null)
                throw new ArgumentNullException(nameof(appCommonSettings));
            
            _appCommonSettings = appCommonSettings;
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
                return JsonSerializer.Deserialize<T>(json, _appCommonSettings.JsonSettings.JsonSerializerOption);
            }

            return null;
        }
    }
}
