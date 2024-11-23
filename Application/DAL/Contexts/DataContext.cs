using Npgsql;
using Dapper;

using Common.Settings;

using DAL.Mapping;
using DAL.Enteties.ClientOperations.Result;
using Common.Secret;

namespace DAL.Contexts
{
    public class DataContext : IDataContext
    {
        public IProcedure Procedure { get; init; }
        public IFunction Function { get; init; }

        public DataContext(ISecretManager secretManager, IAppCommonSettings appCommonSettings)
        {
            if (secretManager == null)
                throw new ArgumentNullException(nameof(secretManager));

            if (appCommonSettings == null) 
                throw new ArgumentNullException(nameof(appCommonSettings));

            SqlMapper.AddTypeHandler(new JsonTypeHandler<ClientOperationResult>(appCommonSettings));
            SqlMapper.AddTypeHandler(new JsonTypeHandler<ClientOperationResult[]>(appCommonSettings));
            SqlMapper.AddTypeHandler(new JsonTypeHandler<ExchangeCourseResult>(appCommonSettings));
            SqlMapper.AddTypeHandler(new JsonTypeHandler<ExchangeCourseResult[]>(appCommonSettings));

            NpgsqlConnection dbConnection = new NpgsqlConnection(secretManager.SecretSettings.DataBaseSettings!.ConnectionString!);

            Procedure = new Procedure(dbConnection);
            Function = new Function(dbConnection);
        }

    }
}
