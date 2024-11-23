using Common.Enums;

namespace Common.Queue.Message.ClientOperation.Result
{
    public class ClientOperationResultMessage : BaseMessage
    {
        public Guid ClientOperationId { get; set; }
        public decimal CurrentTotalAmount { get; set; }
        public decimal TotalAmountOld { get; set; }
        public decimal TotalAmountNew { get; set; }
        public decimal OperationValue { get; set; }
        public decimal? CurrencyCourseSaleValue { get; set; }
        public decimal? CurrencyCoursePurchaseValue { get; set; }
        public CurrencyISONames? ClientAccountCurrencyISOName { get; set; }
        public CurrencyISONames? OperationCurrencyISOName { get; set; }
        public ClientOperationTypes ClientOperationType { get; set; }
        public ClientOperationStatuses ClientOperationStatus { get; set; }
        public ExchangeCourseResultMessage[]? CurrentExchangeCourse { get; set; }
    }
}
