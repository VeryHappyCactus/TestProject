
namespace DAL.Enteties.ClientOperations.Request
{
    public class ClientOperationsRequest
    {
        public Guid client_id { get; set; }
        public ClientDepartmentAddressRequest? department_address { get; set; }

    }
}
