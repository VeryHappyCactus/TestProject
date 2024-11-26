using System.Data;
using System.Text.Json;

using Npgsql;
using NpgsqlTypes;

using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;
using DAL.StoredProcedures.Interfaces;

namespace DAL.StoredProcedures
{
    public partial class ClientOperationProcedure : IClientOperationProcedure
    {
        private NpgsqlConnection _dbConnection;

        public ClientOperationProcedure(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ClientWithdrawOperationResult> WithdrawAsync(ClientWithdrawOperationRequest model)
        {
            try
            {
                var json = JsonSerializer.Serialize(model);

                await _dbConnection.OpenAsync();

                await using var command = new NpgsqlCommand("sp_withdraw", _dbConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    Parameters =
                    {
                        new NpgsqlParameter("msg", NpgsqlDbType.Jsonb)
                        {
                            Value = json,
                            Direction = ParameterDirection.Input
                        }
                    }
                };

                await using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    return new ClientWithdrawOperationResult()
                    {
                        ErrorCode = reader.GetValue("rErrorCode") as int?,
                        ClientOperationId = reader.GetValue("rClientOperationId") as Guid?
                    };
                }
                else
                {
                    throw new Exception("Can not read result from sp_withdraw stored procedure");
                }
            }
            finally
            {
                await _dbConnection.CloseAsync();
            }
        }
    }
}
