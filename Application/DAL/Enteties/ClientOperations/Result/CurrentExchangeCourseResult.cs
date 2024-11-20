using Common.Enums;

namespace DAL.Enteties.ClientOperations.Result
{
    public class CurrentExchangeCourseResult
    {
        public CurrencyISONames CurrencyISOName { get; set; }
        public decimal SaleValue { get; set; }
        public decimal PurchaseValue { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
