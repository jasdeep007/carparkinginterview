using carparkinginterview.DTOs;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace carparkinginterview.Tests
{
    public class ModelValidationTests
    {
        [Theory]
        [InlineData("ABC123", "small", true)] // Valid
        [InlineData("", "small", false)] // Empty registration
        [InlineData("ABC123", "invalid", false)] // Invalid vehicle type
        [InlineData("A", "small", false)] // Too short registration
        public void ParkVehicleRequest_Validation(string reg, string type, bool expectedIsValid)
        {
            // Arrange
            var request = new ParkVehicleRequest
            {
                VehicleReg = reg,
                VehicleType = type
            };
            var context = new ValidationContext(request);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(request, context, results, true);

            // Assert
            Xunit.Assert.Equal(expectedIsValid, isValid);
        }

        [Theory]
        [InlineData("ABC123", true)] // Valid
        [InlineData("", false)] // Empty registration
        [InlineData("A", false)] // Too short registration
        public void ExitVehicleRequest_Validation(string reg, bool expectedIsValid)
        {
            // Arrange
            var request = new ExitVehicleRequest { VehicleReg = reg };
            var context = new ValidationContext(request);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(request, context, results, true);

            // Assert
            Xunit.Assert.Equal(expectedIsValid, isValid);
        }
    }
}
