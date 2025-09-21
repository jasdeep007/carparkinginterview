namespace carparkinginterview.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Registration { get; set; }
        public string Type { get; set; } // Small, Medium, Large
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public int? ParkingSpaceId { get; set; }
        public ParkingSpace ParkingSpace { get; set; }
        public double? Charges { get; set; }
    }
}

