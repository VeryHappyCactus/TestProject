using DAL.Functions;

namespace DAL.Contexts
{
    public interface IFunction
    {
        public IClientOperationFunction CleintOperationFunction { get; init; }
    }
}
