using carparkinginterview.Controllers;
using carparkinginterview.DTOs;
using carparkinginterview.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace carparkinginterview.Tests
{
    public class ParkingControllerTests
    {
        private readonly Mock<IParkingService> _mockParkingService;
        private readonly ParkingController _controller;

        public ParkingControllerTests()
        {
            _mockParkingService = new Mock<IParkingService>();
            _controller = new ParkingController(_mockParkingService.Object);
        }

        [Fact]
        public async Task ParkVehicle_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new ParkVehicleRequest
            {
                VehicleReg = "ABC123",
                VehicleType = "small"
            };

            var expectedResult = new ParkingResult
            {
                VehicleReg = "ABC123",
                SpaceNumber = 1,
                TimeIn = DateTime.UtcNow
            };

            _mockParkingService.Setup(s => s.ParkVehicle(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ParkVehicle(request);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            var returnValue = Xunit.Assert.IsType<ParkingResult>(okResult.Value);
            Xunit.Assert.Equal("ABC123", returnValue.VehicleReg);
        }

        [Fact]
        public async Task ParkVehicle_InvalidOperation_ReturnsBadRequest()
        {
            // Arrange
            var request = new ParkVehicleRequest
            {
                VehicleReg = "ABC123",
                VehicleType = "small"
            };

            _mockParkingService.Setup(s => s.ParkVehicle(request))
                .ThrowsAsync(new InvalidOperationException("Vehicle already parked"));

            // Act
            var result = await _controller.ParkVehicle(request);

            // Assert
            var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
            Xunit.Assert.Equal("Vehicle already parked", badRequestResult.Value);
        }

        [Fact]
        public async Task GetParkingStatus_ReturnsOkResult()
        {
            // Arrange
            var expectedStatus = new ParkingStatus
            {
                AvailableSpaces = 80,
                OccupiedSpaces = 20
            };

            _mockParkingService.Setup(s => s.GetParkingStatus())
                .ReturnsAsync(expectedStatus);

            // Act
            var result = await _controller.GetParkingStatus();

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            var returnValue = Xunit.Assert.IsType<ParkingStatus>(okResult.Value);
            Xunit.Assert.Equal(80, returnValue.AvailableSpaces);
        }

        [Fact]
        public async Task ExitVehicle_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new ExitVehicleRequest { VehicleReg = "ABC123" };
            var expectedResult = new ExitResult
            {
                VehicleReg = "ABC123",
                VehicleCharge = 25.50
            };

            _mockParkingService.Setup(s => s.ExitVehicle(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ExitVehicle(request);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            var returnValue = Xunit.Assert.IsType<ExitResult>(okResult.Value);
            Xunit.Assert.Equal("ABC123", returnValue.VehicleReg);
        }
    }
}
