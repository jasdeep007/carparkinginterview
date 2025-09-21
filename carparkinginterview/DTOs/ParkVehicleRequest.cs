using System.ComponentModel.DataAnnotations;

namespace carparkinginterview.DTOs
{
    public class ParkVehicleRequest : ExitVehicleRequest
    {
        [Required(ErrorMessage = "Vehicle type is required")]
        [RegularExpression("^(small|medium|large)$", ErrorMessage = "Vehicle type must be: small, medium, or large")]
        public required string VehicleType { get; set; }
    }
}
