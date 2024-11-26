using Common.Enums;

namespace Common.Queue.Message.ClientOperation.Result
{
    public class ExchangeCourseResultMessage : BaseMessage
    {
        public CurrencyISONames CurrencyISOName { get; set; }
        public decimal SaleValue { get; set; }
        public decimal PurchaseValue { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
