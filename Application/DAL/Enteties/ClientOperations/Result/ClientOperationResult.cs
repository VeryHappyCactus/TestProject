using Common.Enums;

namespace DAL.Enteties.ClientOperations.Result
{
    public class ClientOperationResult
    {
        public Guid ClientOperationId { get; set; }
        public decimal CurrentTotalAmount { get; set; }
        public decimal TotalAmountOld { get; set; }
        public decimal TotalAmountNew { get; set; }
        public decimal ClientOperationValue { get; set; }
        public decimal? CurrencyCourseSaleValue { get; set; }
        public decimal? CurrencyCoursePurchasValue { get; set; }
        public CurrencyISONames? CurrencyISOName { get; set; }
        public ClientOperationTypes ClientOperationType { get; set; }
        public ClientOperationStatuses ClientOperationStatus { get; set; }
        public CurrentExchangeCourseResult[]? CurrentExchangeCourse { get; set; }
    }
}


