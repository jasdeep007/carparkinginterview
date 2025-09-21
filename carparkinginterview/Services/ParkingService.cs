using carparkinginterview.Data;
using carparkinginterview.DTOs;
using carparkinginterview.Models;
using Microsoft.EntityFrameworkCore;

namespace carparkinginterview.Services
{
    public interface IParkingService
    {
        Task<ParkingResult> ParkVehicle(ParkVehicleRequest request);
        Task<ParkingStatus> GetParkingStatus();
        Task<ExitResult> ExitVehicle(ExitVehicleRequest request);
    }
    public class ParkingService : IParkingService
    {
        private readonly CarParkContext _context;

        public ParkingService(CarParkContext context)
        {
            _context = context;
        }

        public async Task<ParkingResult> ParkVehicle(ParkVehicleRequest request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    // Check if vehicle is already parked
                    var existingVehicle = await _context.Vehicles
                        .FirstOrDefaultAsync(v => v.Registration == request.VehicleReg && v.TimeOut == null);

                    if (existingVehicle != null)
                    {
                        throw new InvalidOperationException("Vehicle is already parked");
                    }

                    // Find available parking space that matches vehicle type
                    var availableSpace = await _context.ParkingSpaces
                        .FirstOrDefaultAsync(ps => !ps.IsOccupied && ps.Size == request.VehicleType);

                    if (availableSpace == null)
                    {
                        throw new InvalidOperationException("No available parking space for this vehicle type");
                    }

                    // Create new vehicle record
                    var vehicle = new Vehicle
                    {
                        Registration = request.VehicleReg,
                        Type = request.VehicleType,
                        TimeIn = DateTime.UtcNow,
                        ParkingSpaceId = availableSpace.Id
                    };

                    // Add vehicle to context
                    _context.Vehicles.Add(vehicle);

                    // Update parking space - EF will handle this in the same transaction
                    availableSpace.IsOccupied = true;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ParkingResult
                    {
                        VehicleReg = vehicle.Registration,
                        SpaceNumber = availableSpace.Id,
                        TimeIn = vehicle.TimeIn
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<ParkingStatus> GetParkingStatus()
        {
            var totalSpaces = await _context.ParkingSpaces.CountAsync();
            var occupiedSpaces = await _context.ParkingSpaces.CountAsync(ps => ps.IsOccupied);
            //var spacesByType = await _context.ParkingSpaces
            //                            .GroupBy(ps => ps.Size)
            //                            .Select(g => new SpaceTypeStatus
            //                            {
            //                                Type = g.Key,
            //                                Total = g.Count(),
            //                                Occupied = g.Count(ps => ps.IsOccupied),
            //                                Available = g.Count(ps => !ps.IsOccupied)
            //                            })
            //                            .ToListAsync();

            return new ParkingStatus
            {
                AvailableSpaces = totalSpaces - occupiedSpaces,
                OccupiedSpaces = occupiedSpaces//,
                //SpacesByType = spacesByType
            };
        }

        public async Task<ExitResult> ExitVehicle(ExitVehicleRequest request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Find the vehicle that is currently parked
                    var vehicle = await _context.Vehicles
                        .Include(v => v.ParkingSpace)
                        .FirstOrDefaultAsync(v => v.Registration == request.VehicleReg && v.TimeOut == null);

                    if (vehicle == null)
                    {
                        throw new InvalidOperationException("Vehicle not found or not currently parked");
                    }

                    // Calculate charge
                    var timeOut = DateTime.UtcNow;
                    var timeIn = vehicle.TimeIn;
                    var duration = timeOut - timeIn;
                    var charge = CalculateCharge(vehicle.Type, duration);

                    // Update vehicle record
                    vehicle.TimeOut = timeOut;

                    // Free up parking space
                    if (vehicle.ParkingSpace != null)
                    {
                        vehicle.ParkingSpace.IsOccupied = false;
                        vehicle.Charges = charge;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ExitResult
                    {
                        VehicleReg = vehicle.Registration,
                        VehicleCharge = charge,
                        TimeIn = timeIn,
                        TimeOut = timeOut
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException("Error processing vehicle exit", ex);
                }
            }
        }

        private double CalculateCharge(string vehicleType, TimeSpan duration)
        {
            //return 100.0; // Placeholder for actual calculation logic
            double ratePerMinute = vehicleType.ToLower() switch
            {
                "small" => 0.10,
                "medium" => 0.20,
                "large" => 0.40,
                _ => throw new ArgumentException("Invalid vehicle type")
            };

            double totalMinutes = duration.TotalMinutes;
            double baseCharge = totalMinutes * ratePerMinute;

            // Add £1 for every 5 minutes
            double additionalCharge = Math.Floor(totalMinutes / 5) * 1.0;

            return Math.Round(baseCharge + additionalCharge, 2);
        }
    }

    public class ParkingResult
    {
        public string VehicleReg { get; set; }
        public int SpaceNumber { get; set; }
        public DateTime TimeIn { get; set; }
    }

    public class ParkingStatus
    {
        public int AvailableSpaces { get; set; }
        public int OccupiedSpaces { get; set; }
        //public List<SpaceTypeStatus> SpacesByType { get; set; }
    }
    public class SpaceTypeStatus
    {
        public string Type { get; set; }
        public int Total { get; set; }
        public int Occupied { get; set; }
        public int Available { get; set; }
    }
    public class ExitResult
    {
        public string VehicleReg { get; set; }
        public double VehicleCharge { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
    }
}
