using DAL.Functions.Interfaces;

namespace DAL
{
    public interface IFunction
    {
        public IClientOperationFunction CleintOperationFunction { get; init; }
    }
}
