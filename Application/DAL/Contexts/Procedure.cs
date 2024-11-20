using Npgsql;

using DAL.StoredProcedures;

namespace DAL.Contexts
{
    public class Procedure : IProcedure
    {
        public IClientOperationProcedure ClientOperationProcedure { get; init; }

        public Procedure(NpgsqlConnection dbConnection)
        {
            ClientOperationProcedure = new ClientOperationProcedure(dbConnection);
        }

    }
}
