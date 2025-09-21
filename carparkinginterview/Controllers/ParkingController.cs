using carparkinginterview.DTOs;
using carparkinginterview.Services;
using Microsoft.AspNetCore.Mvc;

namespace carparkinginterview.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingService _parkingService;
        public ParkingController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpPost]
        public async Task<IActionResult> ParkVehicle([FromBody] ParkVehicleRequest request)
        {
            try
            {
                var result = await _parkingService.ParkVehicle(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetParkingStatus()
        {
            try
            {
                var result = await _parkingService.GetParkingStatus();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("exit")]
        public async Task<IActionResult> ExitVehicle([FromBody] ExitVehicleRequest request)
        {
            try
            {
                var result = await _parkingService.ExitVehicle(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
