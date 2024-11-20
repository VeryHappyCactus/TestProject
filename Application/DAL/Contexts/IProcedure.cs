using DAL.StoredProcedures;

namespace DAL.Contexts
{
    public interface IProcedure
    {
        public IClientOperationProcedure ClientOperationProcedure { get; init; }
    }
}
