using Common.Enums;

namespace Common.Queue.Message.ClientOperation.Result
{
    public class CurrentExchangeCourseResultMessage
    {
        public CurrencyISONames CurrencyISOName { get; set; }
        public decimal SaleValue { get; set; }
        public decimal PurchaseValue { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
