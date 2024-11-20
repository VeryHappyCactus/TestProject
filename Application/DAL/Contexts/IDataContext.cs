
namespace DAL.Contexts
{
    public interface IDataContext
    {
        public IProcedure Procedure { get; init; }
        public IFunction Function { get; init; }
    }
}
