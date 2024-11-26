using Common.Enums;

namespace MediatorHandlers.Handlers.ClientOperations.Result
{
    public class GetExchangeCourseResult
    {
        public CurrencyISONames CurrencyISOName { get; set; }
        public decimal SaleValue { get; set; }
        public decimal PurchaseValue { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
