using DAL.Functions;
using DAL.Functions.Interfaces;
using Npgsql;

namespace DAL.Contexts
{
    public class Function : IFunction
    {
        public IClientOperationFunction CleintOperationFunction { get; init; }

        public Function(NpgsqlConnection dbConnection)
        {
            CleintOperationFunction = new ClientOperationFunction(dbConnection);
        }

    }
}
