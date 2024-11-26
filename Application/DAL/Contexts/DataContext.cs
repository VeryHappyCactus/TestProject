using System.Text.Json;

using Npgsql;
using Dapper;

using DAL.Mapping;
using DAL.Enteties.ClientOperations.Result;

namespace DAL.Contexts
{
    public class DataContext : IDataContext
    {
        public IProcedure Procedure { get; init; }
        public IFunction Function { get; init; }

        public DataContext(string connectionString, JsonSerializerOptions jsonSerializerOption)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (jsonSerializerOption == null) 
                throw new ArgumentNullException(nameof(jsonSerializerOption));

            SqlMapper.AddTypeHandler(new JsonTypeHandler<ClientOperationResult>(jsonSerializerOption));
            SqlMapper.AddTypeHandler(new JsonTypeHandler<ClientOperationResult[]>(jsonSerializerOption));
            SqlMapper.AddTypeHandler(new JsonTypeHandler<ExchangeCourseResult>(jsonSerializerOption));
            SqlMapper.AddTypeHandler(new JsonTypeHandler<ExchangeCourseResult[]>(jsonSerializerOption));

            NpgsqlConnection dbConnection = new NpgsqlConnection(connectionString);

            Procedure = new Procedure(dbConnection);
            Function = new Function(dbConnection);
        }

    }
}
