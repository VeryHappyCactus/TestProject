using System.ComponentModel.DataAnnotations;

namespace Service.Models.ClientOperations.Request
{
    public class ClientOperationsRequest
    {
        public Guid ClientId { get; set; }
        
        [Required]
        public ClientDepartmentAddressRequest? DepartmentAddress { get; set; }
    }
}
