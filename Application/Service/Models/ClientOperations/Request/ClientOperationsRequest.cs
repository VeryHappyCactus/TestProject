
namespace Service.Models.ClientOperations.Request
{
    public class ClientOperationsRequest
    {
        public Guid ClientId { get; set; }
        public ClientDepartmentAddressRequest? DepartmentAddress { get; set; }
    }
}
