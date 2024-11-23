
using System.ComponentModel.DataAnnotations;

namespace Service.Models.ClientOperations.Request
{
    public class ClientDepartmentAddressRequest
    {
        [Required]
        public string? StreetName { get; set; }
        
        [Required]
        public string? BuildingNumber { get; set; }
    }
}
