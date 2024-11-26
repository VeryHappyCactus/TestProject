using DAL.StoredProcedures.Interfaces;

namespace DAL
{
    public interface IProcedure
    {
        public IClientOperationProcedure ClientOperationProcedure { get; init; }
    }
}
