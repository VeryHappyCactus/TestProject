using System.Text.Json;

using Dapper;
using Npgsql;

using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace DAL.Functions
{
    public class ClientOperationFunction : IClientOperationFunction
    {
        private NpgsqlConnection _dbConnection;

        public ClientOperationFunction(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ClientOperationResult?> GetOperationByIdAsync(Guid clientOperationId)
        {
            string json = JsonSerializer.Serialize(new
            {
                client_operation_id = clientOperationId
            });

            IEnumerable<ClientOperationResult> result = await _dbConnection
                .QueryAsync<ClientOperationResult>("select * from fn_GetStatusInfo(@msg::jsonb)", param: new { msg = json });

            return result?.FirstOrDefault();
        }


        public async Task<ClientOperationResult[]?> GetOperationsAsync(ClientOperationsRequest model)
        {
          
            string json = JsonSerializer.Serialize(model);

            IEnumerable<ClientOperationResult[]> result = await _dbConnection
                .QueryAsync<ClientOperationResult[]>("select * from fn_GetStatusInfo(@msg::jsonb)", param: new { msg = json });

            return result?.FirstOrDefault();
          
        }
    }
}
