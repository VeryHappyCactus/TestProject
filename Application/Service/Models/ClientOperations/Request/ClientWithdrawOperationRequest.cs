using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Service.Models.ClientOperations.Request
{
    public class ClientWithdrawOperationRequest
    {
        public Guid ClientId { get; set; }

        [Required]
        public ClientDepartmentAddressRequest? DepartmentAddress { get; set; }

        [Range(100, 100_000)]
        public decimal Amount { get; set; }

        public CurrencyISONames CurrencyISOName { get; set; }
      
    }
}
