using System.ComponentModel.DataAnnotations;

namespace carparkinginterview.DTOs
{
    public class ExitVehicleRequest
    {
        [Required(ErrorMessage = "Vehicle registration is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Vehicle registration must be between 2 and 20 characters")]
        public required string VehicleReg { get; set; }
    }
}
